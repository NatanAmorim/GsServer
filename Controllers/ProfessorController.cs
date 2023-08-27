using System.Net;
using gs_server;
using gs_server.Dtos.Professores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sgd_cms.ControlFlow.Professores;
using sgd_cms.Services.Professores;

namespace sgd_cms.Controllers.Professores;

// [Authorize]
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

  // POST api/[controller]/register
  /// <summary>
  /// Creates a professor.
  /// </summary>
  /// <returns>A newly created professor</returns>
  /// <response code="201">Returns the newly created professor</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="409">Professor already exists.</response>
  [HttpPost("register")]
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
    Result<ResponseProfessorDto, ProfessorErrors?> Result =
       await _professorService.PostAsync(request);

    return Result.Match<ActionResult<ResponseProfessorDto>>(
      Professor => Created(
        "api/Professors/" + Professor.Id, Professor
      ),

      Failed =>
      {
        switch (Failed)
        {
          case ProfessorErrors.MissingRequiredField:
            return BadRequest();
          case ProfessorErrors.DuplicateEntry:
            return BadRequest();
          case ProfessorErrors.InvalidFormat:
            _logger.LogWarning(
             "Falha na crição do professor, {professor} já existe.",
             request.Nome
            );
            ProblemDetails details = new()
            {
              Status = (int)HttpStatusCode.Conflict,
              Type = "PLACEHOLDER",
              Title = "Este professor já existe",
              Detail = "PLACEHOLDER",
            };
            return Conflict(details);
          default:
            throw new NotImplementedException();
        }
      }
    );
  }

}