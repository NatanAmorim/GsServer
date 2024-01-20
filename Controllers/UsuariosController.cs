using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using gs_server.Dtos.Usuarios;
using gs_server.Services.Usuarios;

namespace gs_server.Controllers.Usuarios;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
  private readonly ILogger<UsuariosController> _logger;
  private readonly IUsuarioService _usuarioService;
  public UsuariosController(
    ILogger<UsuariosController> logger,
      IUsuarioService usuarioService
    )
  {
    _logger = logger;
    _usuarioService = usuarioService;
  }

  // GET api/[controller]
  /// <summary>
  /// Returns a list of users.
  /// </summary>
  /// <param name="Page">Page index</param>
  /// <param name="Limit">The numbers of items to return.</param>
  /// <param name="Query">String used for text search in one or more fields</param>
  /// <returns>Returns a list of users.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Page must be int and bigger than zero</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<IEnumerable<ResponseUsuarioDto>>> GetAsync(
    [Required][FromQuery] int Page,
    [FromQuery] int? Limit,
    [FromQuery] string? Query
  )
  {
    if (Page < 1)
    {
      _logger.LogWarning("A página deve ser número inteiro maior que zero.");
      return BadRequest(new { message = "A página deve ser número inteiro maior que zero." });
    }

    _logger.LogInformation(
      "Lendo Usuários: Page {Page}, Limit {Limit}, Query {Query}",
      Page,
      Limit ?? 10,
      Query
    );

    try
    {
      IEnumerable<ResponseLeanUsuarioDto>? usuarios = await _usuarioService.GetAsync(Page, Limit ?? 10, Query);

      int TotalCount = await _usuarioService.CountAsync(Query);
      Response.Headers.Append("X-Total-Count", TotalCount.ToString());

      _logger.LogInformation("Usuários lidos com sucesso");
      return Ok(usuarios);
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Erro lendo Usuários: Page {Page}, Limit {Limit}, Query {Query}",
        Page,
        Limit ?? 10,
        Query
      );
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação");
    }
  }

  // GET api/[controller]/{id}
  /// <summary>
  /// Returns a single user.
  /// </summary>
  /// <param name="Id">ID of user to return</param>
  /// <returns>Returns a single user.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Invalid ID supplied</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">User not found</response>
  [HttpGet("{Id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ResponseUsuarioDto>> FindAsync([FromRoute] int Id)
  {
    _logger.LogInformation("Lendo Usuário com ID {Id}", Id);

    try
    {
      ResponseUsuarioDto? Usuario = await _usuarioService.FindAsync(Id);

      if (Usuario is null)
      {
        _logger.LogWarning("Usuário com ID {Id} não encontrada", Id);
        return NotFound();
      }

      _logger.LogInformation("Usuário lido com sucesso");
      return Ok(Usuario);
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Erro lendo Usuário com ID {Id}",
        Id
      );
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação");
    }
  }

  // Patch: api/[controller]/{id}
  /// <summary>
  /// Update an existing user.
  /// </summary>
  /// <param name="usuarioDto">Updated user object</param>
  /// <response code="204">Confirms the user was updated</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">A user with the specified ID was not found</response>
  [HttpPut]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PutAsync([FromBody] ResponseUsuarioDto usuarioDto)
  {
    _logger.LogInformation(
      "Atualizando Usuário com Id {Id}",
      usuarioDto.Id
    );

    try
    {
      bool isSuccess = await _usuarioService.PutAsync(usuarioDto);

      if (isSuccess)
      {
        _logger.LogInformation(
          "Usuário com Id {Id} atualizado com sucesso",
          usuarioDto.Id
        );
        return NoContent();
      }
      _logger.LogWarning(
        "Usuário com Id {Id} não encontrado",
        usuarioDto.Id
      );
      return NotFound();
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Erro atualizando Usuário com Id {Id}",
        usuarioDto.Id
      );
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação");
    }
  }

  // Delete: api/<ValuesController>/{id}
  /// <summary>
  /// Deletes a specific user.
  /// </summary>
  /// <param name="Id">ID of user that needs to be deleted</param>
  /// <returns></returns>
  /// <response code="204">Confirms the user was deleted</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">A user with the specified ID was not found</response>
  [HttpDelete("{Id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int Id)
  {
    if (Id == 1)
    {
      // Zero is the Id of the master user, the one who owns the system
      // therefore this user cannot be deleted
      _logger.LogWarning("O primeiro usuário não pode ser deletado");
      return BadRequest("O primeiro usuário não pode ser deletado");
    }

    _logger.LogInformation("Procurando Usuário com ID {Id}", Id);

    try
    {
      ResponseUsuarioDto? Usuario = await _usuarioService.FindAsync(Id);

      if (Usuario is null)
      {
        _logger.LogWarning(
          "Usuário com Id {Id} não encontrado",
          Id
        );
        return NotFound();
      }

      _logger.LogInformation(
      "Deletando Usuário com Id {Id}",
      Id
    );
      await _usuarioService.DeleteAsync(Id);

      _logger.LogInformation(
        "Usuário com Id {Id} deletado com sucesso",
        Id
      );
      return NoContent();
    }
    catch (Exception ex)
    {
      _logger.LogError(
        ex,
        "Erro deletando Usuário com Id {Id}",
        Id
      );
      return StatusCode(500, "Ocorreu um erro ao processar a solicitação");
    }
  }
}