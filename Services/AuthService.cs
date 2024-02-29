using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using gs_server.Models;
namespace gs_server.Services;

[Authorize]
public class AuthRpcService : AuthService.AuthServiceBase
{
  private readonly IConfiguration _configuration;
  private readonly DatabaseContext _dbContext;
  private readonly ILogger<AuthRpcService> _logger;
  public AuthRpcService(IConfiguration configuration, ILogger<AuthRpcService> logger, DatabaseContext dbContext)
  {
    _configuration = configuration;
    _dbContext = dbContext;
    _logger = logger;
  }

  [AllowAnonymous]
  public override async Task<LoginResponse> Login(LoginRequest request, ServerCallContext context)
  {
    _logger.LogInformation(
      "Login attempt, searching for User {username}",
      request.Email
    );

    User? User =
      await _dbContext.Users.FirstOrDefaultAsync(
        x => x.Email.ToLower().Equals(request.Email.ToLower())
      );

    if (User is null)
    {
      _logger.LogWarning(
        "Error in login attempt, User {user} and/or incorrect password.",
        request.Email
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Erro na tentativa de login, Usuário e/ou Senha incorreto(a)."
      ));
    }

    bool isPasswordWrong = !VerifyPassword(
      request.Password,
      User.PasswordHash,
      User.PasswordSalt
    );

    if (isPasswordWrong)
    {
      _logger.LogWarning(
        "Error in login attempt, user {user} and/or incorrect password.",
        request.Email
      );

      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Erro na tentativa de login, Usuário e/ou Senha incorreto(a)."
      ));
    }

    string JwtToken = GenerateJwtToken(User);
    string RefreshToken = GenerateRefreshToken();

    RefreshToken refreshTokenEntity = new()
    {
      UserId = User.Id,
      Token = RefreshToken
    };

    // RefreshToken is saved as stateful in the database, since it allows to
    // check if the RefreshToken is still valid and check if it belongs to the
    // user that wants to mint a new JWT.
    _dbContext.RefreshTokens.Add(refreshTokenEntity);

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "Login successful! Email {Email}",
      request.Email
    );

    return new LoginResponse()
    {
      AccessToken = JwtToken,
      RefreshToken = RefreshToken
    };
  }

  public override async Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  [AllowAnonymous]
  public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
  {
    _logger.LogInformation(
      "Trying to create a new user. Email {Email}",
      request.Email
    );

    User? User =
      await _dbContext.Users.FirstOrDefaultAsync(
        x => x.Email.ToLower().Equals(request.Email.ToLower())
      );

    if (User is not null)
    {
      _logger.LogWarning(
        "Failed to create a new User, Email {Email} already exists in the Database.",
        request.Email
      );

      throw new RpcException(new Status(
        StatusCode.AlreadyExists, "Falha ao criar um novo usuário, o usuário já existe"
      ));
    }

    CreatePasswordHash(request.Password, out byte[] generatedPasswordHash, out byte[] generatedPasswordSalt);

    User = new User()
    {
      Email = request.Email,
      Role = "user",
      PasswordHash = generatedPasswordHash,
      PasswordSalt = generatedPasswordSalt,
    };

    _dbContext.Users.Add(User);
    await _dbContext.SaveChangesAsync();

    return new RegisterResponse();
  }

  public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
  {
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "Trying to mint a new JWT with Refresh Token {RefreshToken}.",
      request.RefreshToken
    );

    User? User = await _dbContext.Users.FindAsync(UserId);

    if (User is null)
    {
      _logger.LogWarning("Failed to mint a new JWT with Refresh Token that was provided, User not found.");
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "RefreshToken inválido ou expirado."
      ));
    }

    RefreshToken? RefreshToken =
      await _dbContext.RefreshTokens.FirstOrDefaultAsync(
        x => x.Token.Equals(request.RefreshToken)
      );

    if (RefreshToken is null || RefreshToken.UserId != UserId || RefreshToken.ExpiresIn < DateTime.UtcNow || RefreshToken.IsValid == false)
    {
      _logger.LogWarning(
        "Failed to mint a new JWT, Refresh Token {RefreshToken} Invalid or Expired.",
        request.RefreshToken
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "RefreshToken inválido ou expirado."
      ));
    }

    _logger.LogInformation("New JWT created successfully!");

    string JwtToken = GenerateJwtToken(User);

    return new RefreshTokenResponse()
    {
      AccessToken = JwtToken
    };
  }

  public override async Task<NewPasswordResponse> NewPassword(NewPasswordRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
  {
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "Trying to change password, UserId {UserId}",
      UserId
    );

    User? User = await _dbContext.Users.FindAsync(UserId);

    if (User is null)
    {
      _logger.LogWarning("Password change request failed, User not found.");
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Usuário e/ou Senha incorreto(s)."
      ));
    }

    bool isPasswordWrong = !VerifyPassword(
      request.OldPassword,
      User.PasswordHash,
      User.PasswordSalt
    );

    if (isPasswordWrong)
    {
      _logger.LogWarning(
        "Password change request failed, incorrect username and/or password. UserId {UserId}",
        UserId
      );

      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Usuário e/ou Senha incorreto(s)."
      ));
    }

    CreatePasswordHash(request.NewPassword, out byte[] generatedPasswordHash, out byte[] generatedPasswordSalt);

    // Attach the entity to the context in the modified state
    _dbContext.Users.Attach(User);

    User.PasswordHash = generatedPasswordHash;
    User.PasswordSalt = generatedPasswordSalt;

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return new ChangePasswordResponse();
  }

  private void CreatePasswordHash(
      string password,
      out byte[] generatedPasswordHash,
      out byte[] generatedPasswordSalt
    )
  {
    using var hmac = new HMACSHA512();
    generatedPasswordSalt = hmac.Key;
    generatedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
  }

  private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
  {
    using var hmac = new HMACSHA512(passwordSalt);
    byte[] computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    return computeHash.SequenceEqual(passwordHash);
  }
  private string GenerateJwtToken(
        User user
      )
  {
    List<Claim> claims =
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id!.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    ];

    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
        _configuration.GetSection("Authentication:Schemes:Bearer:Secret").Value!
      )
    );

    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    string issuer = _configuration.GetSection("Authentication:Schemes:Bearer:Issuer").Value!;

    JwtSecurityToken token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddMinutes(15),
      signingCredentials: signingCredentials,
      issuer: issuer
    );

    string jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }

  private static string GenerateRefreshToken()
  {
    string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    return token;
  }
}
