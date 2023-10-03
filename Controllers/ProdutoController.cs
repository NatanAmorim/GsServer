using System.Net;
using gs_server.ControlFlow.Produtos;
using gs_server.Dtos.Produtos;
using gs_server.Services.Produtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gs_server.Controllers.Produtos;


[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProdutoController : ControllerBase
{
  private readonly ILogger<ProdutoController> _logger;
  private readonly IProdutoService _produtoService;
  public ProdutoController(
      ILogger<ProdutoController> logger,
      IProdutoService produtoService
    )
  {
    _logger = logger;
    _produtoService = produtoService;
  }

  // GET api/[controller]
  /// <summary>
  /// Returns a list of ResponseProdutoDto.
  /// </summary>
  /// <param name="Limit">The numbers of items to return.</param>
  /// <param name="Query">String used for text search in one or more fields</param>
  /// <returns>Returns a list of ResponseProdutoDto.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Page must be int and bigger than zero</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public async Task<ActionResult<IEnumerable<ResponseProdutoDto>>> GetAsync(
    [FromQuery] int? Limit,
    [FromQuery] string? Query
  )
  {
    _logger.LogInformation(
      "Lendo Produtos: Limit {Limit}, Query {Query}",
      Limit ?? 10,
      Query
    );

    Result<IEnumerable<ResponseProdutoDto>, ProdutoErrors> Result =
       await _produtoService.GetAsync(Limit ?? 10, Query);

    return Result.Match<ActionResult<IEnumerable<ResponseProdutoDto>>>(
      Produtos => Ok(Produtos),

      Error => Error switch
      {
        ProdutoErrors.MissingRequiredField => BadRequest(),
        ProdutoErrors.InvalidFormat => BadRequest(),
        ProdutoErrors.RecordNotFound => NotFound(),
        _ => throw new Exception("Erro não tratado listando Produto"),
      }
    );
  }

  // GET api/[controller]/{id}
  /// <summary>
  /// Returns a single produto.
  /// </summary>
  /// <param name="Id">ID of produto to return</param>
  /// <returns>Returns a single produto.</returns>
  /// <response code="200">Successful operation</response>
  /// <response code="400">Invalid ID supplied</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">Produto not found</response>
  [HttpGet("{Id}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<ResponseProdutoDto>> FindAsync([FromRoute] int Id)
  {
    _logger.LogInformation("Lendo Produto com ID={Id}", Id);

    Result<ResponseProdutoDto, ProdutoErrors> Result =
       await _produtoService.FindAsync(Id);

    return Result.Match<ActionResult<ResponseProdutoDto>>(
      Produto => Ok(Produto),

      Error => Error switch
      {
        ProdutoErrors.MissingRequiredField => BadRequest(),
        ProdutoErrors.InvalidFormat => BadRequest(),
        ProdutoErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Produto com Id={Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
        _ => throw new Exception("Erro não tratado procurando Produto"),
      }
    );
  }

  // POST api/[controller]
  /// <summary>
  /// Creates a produto.
  /// </summary>
  /// <returns>A newly created produto</returns>
  /// <response code="201">Returns the newly created produto</response>
  /// <response code="400">Invalid request body</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="409">Produto already exists.</response>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  [ProducesResponseType(StatusCodes.Status409Conflict)]
  public async Task<ActionResult<ResponseProdutoDto>> PostAsync(
      [FromBody] CreateProdutoDto request
    )
  {
    _logger.LogInformation("Criando produto");
    Result<ResponseProdutoDto, ProdutoErrors> Result =
       await _produtoService.PostAsync(request);

    return Result.Match<ActionResult<ResponseProdutoDto>>(
      Produto => Created(
        "api/Produtos/" + Produto.Id, Produto
      ),

      Error => Error switch
      {
        ProdutoErrors.MissingRequiredField => BadRequest(),
        ProdutoErrors.InvalidFormat => BadRequest(),
        ProdutoErrors.DuplicateEntry => Conflict(
          new ProblemDetails()
          {
            Status = (int)HttpStatusCode.Conflict,
            Type = "PLACEHOLDER",
            Title = $"Erro Produto já existe",
            Detail = "PLACEHOLDER",
          }
        ),
        _ => throw new Exception("Erro não tratado criando Produto"),
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
    [FromBody] ResponseProdutoDto request
  )
  {
    _logger.LogInformation(
      "Atualizando Produto com Id={Id}",
      request.Id
    );
    ProdutoErrors? Error = await _produtoService.PutAsync(request);

    if (Error is null)
    {
      _logger.LogInformation(
        "Produto com Id={Id} atualizado com sucesso",
        request.Id
      );
      return NoContent();
    }

    return Error switch
    {
      ProdutoErrors.MissingRequiredField => BadRequest(),
      ProdutoErrors.InvalidFormat => BadRequest(),
      ProdutoErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Produto com Id={request.Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
      _ => throw new Exception("Erro não tratado editando Produto"),
    };
  }

  // Delete: api/<ValuesController>/{id}
  /// <summary>
  /// Deletes a specific produto.
  /// </summary>
  /// <param name="Id">ID of produto that needs to be deleted</param>
  /// <returns></returns>
  /// <response code="204">Confirms the produto was deleted</response>
  /// <response code="401">Invalid authentication credentials</response>
  /// <response code="403">You are not allowed access to this request</response>
  /// <response code="404">A produto with the specified ID was not found</response>
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

    ProdutoErrors? Error = await _produtoService.DeleteAsync(Id);

    if (Error is null)
    {
      _logger.LogInformation(
        "Produto com Id={Id} removido com sucesso",
        Id
      );
      return NoContent();
    }

    return Error switch
    {
      ProdutoErrors.MissingRequiredField => BadRequest(),
      ProdutoErrors.InvalidFormat => BadRequest(),
      ProdutoErrors.RecordNotFound => NotFound(
        new ProblemDetails()
        {
          Status = (int)HttpStatusCode.NotFound,
          Type = "PLACEHOLDER",
          Title = $"Produto(a) com Id={Id}, não encontrado(a)",
          Detail = "PLACEHOLDER",
        }
      ),
      _ => throw new Exception("Erro não tratado removendo Produto"),
    };
  }
}