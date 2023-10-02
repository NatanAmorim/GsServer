using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gs_server.Dtos.Auth;
using gs_server.Dtos.Usuarios;
using gs_server.Services.Auth;

namespace gs_server.Controllers.Auth;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{

  private readonly ILogger<AuthController> _logger;
  private readonly IAuthService _authService;

  public AuthController(
    ILogger<AuthController> logger,
    IAuthService authService
  )
  {
    _logger = logger;
    _authService = authService;
  }

  // POST api/[controller]/register
  /// <summary>
  /// Creates a user.
  /// </summary>
  /// <returns>A newly created user</returns>
  /// <response code="201">Returns the newly created user</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="409">User already exists.</response>
  [HttpPost("register")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<ActionResult<ResponseUsuarioDto>> PostAsync([FromBody] CreateUsuarioDto usuarioDto)
  {
    _logger.LogInformation("Criando usuário");
    ResponseUsuarioDto? Usuario = await _authService.PostAsync(usuarioDto);

    if (Usuario is null)
    {
      _logger.LogWarning(
       "Falha na crição do usuário, {user} já existe.",
       usuarioDto.Email
      );

      return Conflict("Usuário já existe");
    }

    return Created(
      "api/Usuarios/" + Usuario.Id, Usuario
    );
  }

  // POST api/[controller]/register-first-user
  /// <summary>
  /// Creates the first user.
  /// </summary>
  /// <returns>A newly created user</returns>
  /// <response code="201">Returns the newly created user</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="409">User already exists.</response>
  [HttpPost("register-first-user"), AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<ActionResult<ResponseUsuarioDto>> PostFirstAsync([FromBody] CreateUsuarioDto usuarioDto)
  {
    _logger.LogInformation("Criando o primeiro usuário");
    ResponseUsuarioDto? Usuario = await _authService.PostFirstAsync(usuarioDto);

    if (Usuario is null)
    {
      _logger.LogWarning(
       "Falha na crição do primeiro usuário, primeiro usuário já existe."
      );

      return Conflict("Primeiro usuário já existe.");
    }

    _logger.LogInformation(
      "Usuário criado com sucesso, api/Usuarios/{id} \n {user}",
      Usuario.Id,
      Usuario
    );

    return Created(
      "api/Usuarios/" + Usuario.Id, Usuario
    );
  }

  // POST api/[controller]/login
  /// <summary>
  /// Logs user into the system
  /// </summary>
  /// <returns>ResponseLoginDto</returns>
  /// <response code="200">successful operation</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  [HttpPost("login"), AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<ResponseLoginDto>> login([FromBody] RequestLoginDto loginDto)
  {
    _logger.LogInformation(
      "Tentativa de login, Procurando usuário {email}",
      loginDto.Email
    );
    ResponseLoginDto? response = await _authService.LoginAsync(loginDto);

    if (response is null)
    {
      _logger.LogWarning(
        "Tentativa de login, usuário {user} e/ou Senha incorreto(s)!",
        loginDto.Email
      );

      return Unauthorized("Usuário e/ou Senha incorreto(s)!");
    }

    _logger.LogInformation(
      "Tentativa de login, {email} logado com sucesso",
      loginDto.Email
    );

    return response;
  }

  // PUT api/[controller]/change-password
  /// <summary>
  /// Update the currently logged-in user password.
  /// </summary>
  /// <returns>String</returns>
  /// <response code="200">Password changed</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">User not found</response>
  [HttpPut("change-password")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> ChangePassword(
      [FromBody] NovaSenhaDto request
    )
  {
    int UserId = Int32.Parse(
      HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!
    );

    _logger.LogInformation(
      "Tentativa de troca de senha Id:{Id}",
      UserId
    );
    bool isSuccess = await _authService.ChangePassword(request.SenhaAntiga, request.SenhaNova);

    if (isSuccess)
    {
      _logger.LogInformation("Senha alterada com sucesso!");
      return Ok("Senha alterada com sucesso!");
    }

    _logger.LogWarning("Erro na validação do dados.");

    return Unauthorized("Erro na validação do dados.");
  }

  // POST api/[controller]/refresh-token
  /// <summary>
  /// Mints a new access token using a valid refresh token.
  /// </summary>
  /// <returns>String</returns>
  /// <response code="200">successful operation</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  [HttpPost("refresh-token"), AllowAnonymous]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<string>> RefreshToken([FromBody] string refreshToken)
  {
    _logger.LogInformation(
      "Tentando criar um novo token jwt com Refresh Token:{refreshToken}",
      refreshToken
    );

    string? token = await _authService.RefreshToken(refreshToken);

    if (token is null)
    {
      _logger.LogWarning("Refresh Token inválido ou expirado.");
      return Unauthorized("Invalid or expired Refresh Token.");
    }

    _logger.LogInformation(
      "Novo JWT Token criado com sucesso! {token}",
      token
    );

    return Ok(token);
  }
}