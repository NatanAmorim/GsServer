using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using gs_server.Dtos.Auth;
using gs_server.Dtos.Usuarios;
using gs_server.Models.Usuarios;
using gs_server.Models.RefreshTokens;

namespace gs_server.Services.Auth;

public class AuthService : IAuthService
{
  private readonly ILogger<AuthService> _logger;
  private readonly IConfiguration _configuration;
  private readonly IHttpContextAccessor? _httpContextAccessor;
  private readonly DataBaseContext _dbContext;
  private readonly IMapper _mapper;
  public AuthService(
    ILogger<AuthService> logger,
    IConfiguration configuration,
    IHttpContextAccessor? httpContextAccessor,
    DataBaseContext dbContext,
    IMapper mapper
  )
  {
    _logger = logger;
    _configuration = configuration;
    _httpContextAccessor = httpContextAccessor;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public async Task<ResponseUsuarioDto?> PostAsync(CreateUsuarioDto usuarioDto)
  {
    Usuario? usuario =
      await _dbContext.Usuarios.FirstOrDefaultAsync(
        x => x.Email.ToLower().Equals(usuarioDto.Email.ToLower())
      );

    if (usuario is not null)
    {
      return null;
    }

    CreatePasswordHash(usuarioDto.Senha, out byte[] senhaHashGerada, out byte[] senhaSaltGerada);

    usuarioDto.CreatedBy =
        _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    Usuario novoUsuario = _mapper.Map<Usuario>(usuarioDto);

    novoUsuario.SenhaHash = senhaHashGerada;
    novoUsuario.SenhaSalt = senhaSaltGerada;

    _dbContext.Usuarios.Add(novoUsuario);
    await _dbContext.SaveChangesAsync();

    return _mapper.Map<ResponseUsuarioDto>(novoUsuario);
  }

  public async Task<ResponseUsuarioDto?> PostFirstAsync(CreateUsuarioDto usuarioDto)
  {
    if (_dbContext.Usuarios.Any())
    {
      return null;
    }

    CreatePasswordHash(usuarioDto.Senha, out byte[] senhaHashGerada, out byte[] senhaSaltGerada);

    usuarioDto.CreatedBy = "1";

    Usuario novoUsuario = _mapper.Map<Usuario>(usuarioDto);

    novoUsuario.SenhaHash = senhaHashGerada;
    novoUsuario.SenhaSalt = senhaSaltGerada;

    _dbContext.Usuarios.Add(novoUsuario);
    await _dbContext.SaveChangesAsync();

    return _mapper.Map<ResponseUsuarioDto>(novoUsuario);
  }

  public async Task<ResponseLoginDto?> LoginAsync(RequestLoginDto loginDto)
  {
    Usuario? usuario =
      await _dbContext.Usuarios.FirstOrDefaultAsync(
        x => x.Email.ToLower().Equals(loginDto.Email.ToLower())
      );

    if (usuario is null)
    {
      return null;
    }

    bool isPasswordWrong = !VerifyPassword(
      loginDto.Senha,
      usuario.SenhaHash,
      usuario.SenhaSalt
    );

    if (isPasswordWrong)
    {
      return null;
    }

    string JwtToken = GenerateJwtToken(usuario);
    string RefreshToken = GenerateRefreshToken();

    RefreshToken refreshTokenEntity = new RefreshToken()
    {
      UserId = usuario.Id,
      Token = RefreshToken
    };

    // RefreshToken é salvo de forma stateful no cadastro do usuário
    // permite conferir se o RefreshToken ainda é valido ou se foi invalidado
    // permite conferir se esse RefreshToken pertence a o usuário que quer cunhar um novo JwT token
    _dbContext.RefreshTokens.Add(refreshTokenEntity);

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return new ResponseLoginDto()
    {
      AcessToken = JwtToken,
      RefreshToken = RefreshToken
    };
  }

  public async Task<bool> ChangePassword(string OldPassword, string NewPassword)
  {
    int UserId = Int32.Parse(
      _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    Usuario? usuario = await _dbContext.Usuarios.FindAsync(UserId);

    if (usuario is null)
    {
      return false;
    }

    bool isPasswordWrong = !VerifyPassword(
      OldPassword,
      usuario.SenhaHash,
      usuario.SenhaSalt
    );

    if (isPasswordWrong)
    {
      return false;
    }

    CreatePasswordHash(NewPassword, out byte[] senhaHashGerada, out byte[] senhaSaltGerada);

    // Attach the entity to the context in the modified state
    _dbContext.Usuarios.Attach(usuario);

    usuario.SenhaHash = senhaHashGerada;
    usuario.SenhaSalt = senhaSaltGerada;

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return true;
  }

  public async Task<string?> RefreshToken(string refreshToken)
  {
    if (refreshToken is null)
    {
      return null;
    }

    int UserId = Int32.Parse(
      _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    Usuario? usuario = await _dbContext.Usuarios.FindAsync(UserId);

    if (usuario is null)
    {
      return null;
    }

    RefreshToken? refreshTokenEntity =
      await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token.Equals(refreshToken));

    if (refreshTokenEntity is null)
    {
      return null;
    }

    if (refreshTokenEntity.ExpiraEm < DateTime.UtcNow)
    {
      return null;
    }

    if (refreshTokenEntity.IsValid == false)
    {
      return null;
    }

    string JwtToken = GenerateJwtToken(usuario);

    return JwtToken;
  }

  private void CreatePasswordHash(
      string senha,
      out byte[] senhaHashGerada,
      out byte[] senhaSaltGerada
    )
  {
    using (var hmac = new HMACSHA512())
    {
      senhaSaltGerada = hmac.Key;
      senhaHashGerada = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
    }
  }

  private bool VerifyPassword(
      string senha,
      byte[] senhaHash,
      byte[] senhaSalt
    )
  {
    using (var hmac = new HMACSHA512(senhaSalt))
    {
      byte[] computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));
      return computeHash.SequenceEqual(senhaHash);
    }
  }

  private string GenerateJwtToken(
      Usuario usuario
    )
  {
    List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id!.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nome)
    };

    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
          _configuration.GetSection("Authentication:Schemes:Bearer:Secret").Value!
        )
      );

    SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

    // TODO remover audiencia
    // List<string> audiences = _configuration.GetSection("Authentication:Schemes:Bearer:Audiences").Get<List<string>>()!;
    string issuer = _configuration.GetSection("Authentication:Schemes:Bearer:Issuer").Value!;

    JwtSecurityToken token = new JwtSecurityToken(
      claims: claims,
      expires: DateTime.Now.AddHours(8),
      signingCredentials: signingCredentials,
      issuer: issuer
    // TODO remover audiencia
    // audience: audiences[0]
    );

    string jwt = new JwtSecurityTokenHandler().WriteToken(token);

    return jwt;
  }

  private string GenerateRefreshToken()
  {
    string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    return token;
  }


}