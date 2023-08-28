using System.Net;
using gs_server;
using gs_server.Dtos.Professores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgd_cms.ControlFlow.Professores;
using sgd_cms.Services.Professores;

namespace sgd_cms.Controllers.Professores;

// [Authorize] //TODO
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProfessorController : ControllerBase
{
  private readonly ILogger<ProfessorController> _logger;
  private readonly IProfessorService _professorService;
  public ProfessorController(
      ILogger<ProfessorController> logger,
      IProfessorService professorService
    )
  {
    _logger = logger;
    _professorService = professorService;
  }

  // GET api/[controller]
  /// <summary>
  /// Returns a list of ResponseProfessorDto.
  /// </summary>
  /// <param name="Limit">The numbers of items to return.</param>
  /// <param name="Query">String used for text search in one or more fields</param>
  /// <returns>Returns a list of ResponseProfessorDto.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Page must be int and bigger than zero</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<IEnumerable<ResponseProfessorDto>>> GetAsync(
    [FromQuery] int? Limit,
    [FromQuery] string? Query
  )
  {
    _logger.LogInformation(
      "Lendo Professores: Limit {Limit}, Query {Query}",
      Limit ?? 10,
      Query
    );

    Result<IEnumerable<ResponseProfessorDto>, ProfessorErrors> Result =
       await _professorService.GetAsync(Limit ?? 10, Query);

    return Result.Match<ActionResult<IEnumerable<ResponseProfessorDto>>>(
      Professores => Ok(Professores),

      Error => Error switch
      {
        ProfessorErrors.MissingRequiredField => BadRequest(),
        ProfessorErrors.InvalidFormat => BadRequest(),
        ProfessorErrors.RecordNotFound => NotFound(),
        _ => throw new Exception("Erro não tratado listando Professor"),
      }
    );
  }

  // GET api/[controller]/{id}
  /// <summary>
  /// Returns a single professor.
  /// </summary>
  /// <param name="Id">ID of professor to return</param>
  /// <returns>Returns a single professor.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Invalid ID supplied</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">Professor not found</response>
  [HttpGet("{Id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ResponseProfessorDto>> FindAsync([FromRoute] int Id)
  {
    _logger.LogInformation("Lendo Professor com ID={Id}", Id);

    Result<ResponseProfessorDto, ProfessorErrors> Result =
       await _professorService.FindAsync(Id);

    return Result.Match<ActionResult<ResponseProfessorDto>>(
      Professor => Ok(Professor),

      Error => Error switch
      {
        ProfessorErrors.MissingRequiredField => BadRequest(),
        ProfessorErrors.InvalidFormat => BadRequest(),
        ProfessorErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Professor com Id={Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
        _ => throw new Exception("Erro não tratado procurando Professor"),
      }
    );
  }

  // POST api/[controller]
  /// <summary>
  /// Creates a professor.
  /// </summary>
  /// <returns>A newly created professor</returns>
  /// <response code="201">Returns the newly created professor</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="409">Professor already exists.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<ActionResult<ResponseProfessorDto>> PostAsync(
      [FromBody] CreateProfessorDto request
    )
  {
    _logger.LogInformation("Criando professor");
    Result<ResponseProfessorDto, ProfessorErrors> Result =
       await _professorService.PostAsync(request);

    return Result.Match<ActionResult<ResponseProfessorDto>>(
      Professor => Created(
        "api/Professors/" + Professor.Id, Professor
      ),

      Error => Error switch
      {
        ProfessorErrors.MissingRequiredField => BadRequest(),
        ProfessorErrors.InvalidFormat => BadRequest(),
        ProfessorErrors.DuplicateEntry => Conflict(
          new ProblemDetails()
          {
            Status = (int)HttpStatusCode.Conflict,
            Type = "PLACEHOLDER",
            Title = $"Erro Professor com CPF={request.Cpf} já existe",
            Detail = "PLACEHOLDER",
          }
        ),
        _ => throw new Exception("Erro não tratado criando Professor"),
      }
    );
  }

  // Patch: api/[controller]/{id}
  /// <summary>
  /// Update an existing company.
  /// </summary>
  /// <param name="request">Updated company object</param>
  /// <response code="204">Confirms the company was updated</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">A company with the specified ID was not found</response>
  [HttpPut]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> PutAsync(
    [FromBody] ResponseProfessorDto request
  )
  {
    _logger.LogInformation(
      "Atualizando Professor com Id={Id}",
      request.Id
    );
    ProfessorErrors? Error = await _professorService.PutAsync(request);

    if (Error is null)
    {
      _logger.LogInformation(
        "Professor com Id={Id} atualizado com sucesso",
        request.Id
      );
      return NoContent();
    }

    return Error switch
    {
      ProfessorErrors.MissingRequiredField => BadRequest(),
      ProfessorErrors.InvalidFormat => BadRequest(),
      ProfessorErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Professor com Id={request.Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
      _ => throw new Exception("Erro não tratado editando Professor"),
    };
  }

  // Delete: api/<ValuesController>/{id}
  /// <summary>
  /// Deletes a specific professor.
  /// </summary>
  /// <param name="Id">ID of professor that needs to be deleted</param>
  /// <returns></returns>
  /// <response code="204">Confirms the professor was deleted</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">A professor with the specified ID was not found</response>
  [HttpDelete("{Id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> DeleteAsync([FromRoute] int Id)
  {
    _logger.LogInformation(
      "Deletando Empresa com Id={Id}",
      Id
    );

    ProfessorErrors? Error = await _professorService.DeleteAsync(Id);

    if (Error is null)
    {
      _logger.LogInformation(
        "Professor com Id={Id} removido com sucesso",
        Id
      );
      return NoContent();
    }

    return Error switch
    {
      ProfessorErrors.MissingRequiredField => BadRequest(),
      ProfessorErrors.InvalidFormat => BadRequest(),
      ProfessorErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Professor(a) com Id={Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
      _ => throw new Exception("Erro não tratado removendo Professor"),
    };
  }
}