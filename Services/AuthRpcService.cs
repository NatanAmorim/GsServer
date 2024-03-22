using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using gs_server.Models;
using gs_server.Protobufs;
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

    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    _logger.LogInformation(
      "({TraceIdentifier}) Login attempt, searching for User {UserId}.",
      RequestTracerId,
      0 // TODO get something here, maybe phone like (18) XXXXX-3114
    );

    UserModel? User =
      await _dbContext.Users.FirstOrDefaultAsync(
        x => x.Email.Equals(request.Email.Trim().ToLower())
      );

    if (User is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error in login attempt, Incorrect User and/or incorrect password.",
        RequestTracerId
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Erro na tentativa de login, Usuário e/ou Senha incorreto."
      ));
    }

    bool isPasswordCorrect = VerifyPassword(
      request.Password,
      User.PasswordHash,
      User.PasswordSalt
    );

    if (isPasswordCorrect == false)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Error in login attempt, incorrect User and/or incorrect password.",
        RequestTracerId
      );

      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Erro na tentativa de login, Usuário e/ou Senha incorreto."
      ));
    }

    string JwtToken = GenerateJwtToken(User);
    string RefreshToken = GenerateRefreshToken();

    RefreshTokenModel refreshTokenEntity = new()
    {
      UserId = User.UserId,
      Token = RefreshToken
    };

    // RefreshToken is saved as stateful in the database, since it allows to
    // check if the RefreshToken is still valid and check if it belongs to the
    // User that wants to mint a new JWT.
    _dbContext.RefreshTokens.Add(refreshTokenEntity);

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    _logger.LogInformation(
      "({TraceIdentifier}) Login successful.",
      RequestTracerId
    );

    return new LoginResponse
    {
      AccessToken = JwtToken,
      RefreshToken = RefreshToken
    };
  }

  public override Task<LogoutResponse> Logout(LogoutRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    _logger.LogInformation(
      "({TraceIdentifier}) User to requested a new User. RefreshToken {RefreshToken}.",
      RequestTracerId,
      request.RefreshToken
    );
    throw new NotImplementedException();
  }

  [AllowAnonymous]
  public override async Task<RegisterResponse> Register(RegisterRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    _logger.LogInformation(
      "({TraceIdentifier}) Trying to create a new User.",
      RequestTracerId
    );

    UserModel? User =
      await _dbContext.Users.FirstOrDefaultAsync(
        x => x.Email.Equals(request.Email.Trim().ToLower())
      );

    if (User is not null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Failed to create a new User, Email already exists in the Database.",
        RequestTracerId
      );

      throw new RpcException(
        new Status(
          StatusCode.AlreadyExists,
          "Falha ao criar um novo usuário, o usuário já existe"
        )
      );
    }

    CreatePasswordHash(request.Password, out byte[] generatedPasswordHash, out byte[] generatedPasswordSalt);

    User = new UserModel
    {
      Email = request.Email.Trim().ToLower(),
      Role = "user",
      PasswordHash = generatedPasswordHash,
      PasswordSalt = generatedPasswordSalt,
    };

    _dbContext.Users.Add(User);
    await _dbContext.SaveChangesAsync();

    /// DON'T log sensitive information
    _logger.LogInformation(
      "({TraceIdentifier}) User {UserId} registered",
      RequestTracerId,
      User.UserId
    );

    return new RegisterResponse();
  }

  public override async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) {UserId} minting a new JWT.",
      RequestTracerId,
      UserId
    );

    UserModel? User = await _dbContext.Users.FindAsync(UserId);

    if (User is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) minting JWT failed, User not found.",
        RequestTracerId
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "RefreshToken inválido ou expirado."
      ));
    }

    RefreshTokenModel? RefreshToken =
      await _dbContext.RefreshTokens.FirstOrDefaultAsync(
        x => x.Token.Equals(request.RefreshToken)
      );

    if (RefreshToken is null || RefreshToken.UserId != UserId || RefreshToken.ExpiresIn < DateTime.UtcNow || RefreshToken.IsValid == false)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) Failed to mint a new JWT, Refresh Token {RefreshToken} Invalid or Expired.",
        RequestTracerId,
        request.RefreshToken
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "RefreshToken inválido ou expirado."
      ));
    }

    _logger.LogInformation(
      "({TraceIdentifier}) New JWT minted successfully",
      RequestTracerId
    );

    string JwtToken = GenerateJwtToken(User);

    return new RefreshTokenResponse
    {
      AccessToken = JwtToken
    };
  }

  public override Task<NewPasswordResponse> NewPassword(NewPasswordRequest request, ServerCallContext context)
  {
    throw new NotImplementedException();
  }

  public override async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
  {
    string RequestTracerId = context.GetHttpContext().TraceIdentifier;
    int UserId = int.Parse(
      context.GetHttpContext().User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "({TraceIdentifier}) changing password, UserId {UserId}",
      RequestTracerId,
      UserId
    );

    UserModel? User = await _dbContext.Users.FindAsync(UserId);

    if (User is null)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) password change failed, User not found.",
        RequestTracerId
      );
      throw new RpcException(new Status(
        StatusCode.Unauthenticated, "Usuário e/ou Senha incorreto(s)."
      ));
    }

    bool isPasswordCorrect = VerifyPassword(
      request.OldPassword,
      User.PasswordHash,
      User.PasswordSalt
    );

    if (isPasswordCorrect == false)
    {
      _logger.LogWarning(
        "({TraceIdentifier}) password change failed, incorrect password. UserId {UserId}",
        RequestTracerId,
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

  private static void CreatePasswordHash(
      string password,
      out byte[] generatedPasswordHash,
      out byte[] generatedPasswordSalt
    )
  {
    using HMACSHA512 hmac = new();
    generatedPasswordSalt = hmac.Key;
    generatedPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
  }

  private static bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
  {
    using HMACSHA512 hmac = new(passwordSalt);
    byte[] computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    return computeHash.SequenceEqual(passwordHash);
  }
  private string GenerateJwtToken(
        UserModel User
      )
  {
    List<Claim> claims =
    [
        new Claim(ClaimTypes.NameIdentifier, User.UserId!.ToString()),
        new Claim(ClaimTypes.Email, User.Email)
    ];

    SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(
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
