
using System.Security.Claims;
using AutoMapper;
using gs_server.ControlFlow.Produtos;
using gs_server.Dtos.Produtos;
using gs_server.Models.Produtos;

namespace gs_server.Services.Produtos;

public class ProdutoService : IProdutoService
{
  private readonly ILogger<ProdutoService> _logger;
  private readonly IHttpContextAccessor? _httpContextAccessor;
  private readonly DataBaseContext _dbContext;
  private readonly IMapper _mapper;
  public ProdutoService(
      ILogger<ProdutoService> logger,
      IHttpContextAccessor? httpContextAccessor,
      DataBaseContext dbContext,
      IMapper mapper
    )
  {
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<ResponseProdutoDto>, ProdutoErrors>> GetAsync(int Limit, string? Query)
  {
    if (Limit < 1)
    {
      _logger.LogWarning("A página deve ser número inteiro maior que zero.");
      return ProdutoErrors.InvalidFormat;
    }

    List<Produto> Produtos = new List<Produto>();

    if (string.IsNullOrEmpty(Query))
    {
      Produtos =
       await _dbContext.Produtos
       .Include(x => x.Variacoes)
      //  .Skip((Page - 1) * Limit)
      //  .Take(Limit)
       .ToListAsync();
    }
    else
    {
      // produtos =
      //    await _dbContext.Produtos
      //    .Where(p => p.SearchVector.Matches(EF.Functions.ToTsQuery("portuguese", Query)))
      //    .Skip((Page - 1) * Limit)
      //    .Take(Limit)
      //    .ToListAsync();
    }

    IEnumerable<ResponseProdutoDto> response =
      Produtos.Select(produto => _mapper.Map<ResponseProdutoDto>(produto));

    return response.ToList(); // TODO fix, ToList() should not be necessary, removing task seems to work, but WHY?
  }

  public async Task<Result<ResponseProdutoDto, ProdutoErrors>> FindAsync(int Id)
  {
    Produto? Produto = await _dbContext.Produtos
      .Where(x => x.Id == Id)
      .Include(x => x.Variacoes)
      .FirstOrDefaultAsync();

    if (Produto is null)
    {
      _logger.LogWarning(
        "Produto com Id={Id}, não encontrado(a)",
        Id
      );
      return ProdutoErrors.RecordNotFound;
    }

    return _mapper.Map<ResponseProdutoDto>(Produto);
  }

  public async Task<Result<ResponseProdutoDto, ProdutoErrors>> PostAsync(CreateProdutoDto produtoDto)
  {
    produtoDto.CreatedBy =
      _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    Produto produto = _mapper.Map<Produto>(produtoDto);

    // if (false) //TODO see if a Produto with the same Produto.CPF already exists
    // {
    //   _logger.LogWarning(
    //     "Erro Produto com CPF={CPF} já existe",
    //     Produto.Cpf
    //   );
    //   return ProdutoErrors.DuplicateEntry;
    // }

    _dbContext.Produtos.Add(produto);
    await _dbContext.SaveChangesAsync();

    return _mapper.Map<ResponseProdutoDto>(produto);
  }

  public async Task<ProdutoErrors?> PutAsync(ResponseProdutoDto produtoDto)
  {
    Produto? Produto = await _dbContext.Produtos
                .Where(x => x.Id == produtoDto.Id)
                .Include(x => x.Variacoes)
                .FirstOrDefaultAsync();

    if (Produto is null)
    {
      _logger.LogWarning(
        "Produto com Id={Id}, não encontrado(a)",
        produtoDto.Id
      );
      return ProdutoErrors.RecordNotFound;
    }

    // Attach the entity to the context in the modified state
    _dbContext.Produtos.Attach(Produto);

    Produto.Nome = produtoDto.Nome;
    // Produto.Variacoes = produtoDto.Variacoes; // TODO

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return null;
  }

  public async Task<ProdutoErrors?> DeleteAsync(int Id)
  {
    Produto? Produto = await _dbContext.Produtos
                .Where(x => x.Id == Id)
                .Include(x => x.Variacoes)
                .FirstOrDefaultAsync();

    if (Produto is null)
    {
      _logger.LogWarning(
        "Produto com Id={Id}, não encontrado(a)",
        Id
      );
      return ProdutoErrors.RecordNotFound;
    }

    _dbContext.Produtos.Remove(Produto);
    await _dbContext.SaveChangesAsync();

    return null;
  }
}