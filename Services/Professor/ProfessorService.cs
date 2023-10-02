using System.Security.Claims;
using AutoMapper;
using gs_server;
using gs_server.Dtos.Professores;
using gs_server.Models.Professores;
using gs_server.ControlFlow.Professores;

namespace gs_server.Services.Professores;

public class ProfessorService : IProfessorService
{
  private readonly ILogger<ProfessorService> _logger;
  private readonly IHttpContextAccessor? _httpContextAccessor;
  private readonly DataBaseContext _dbContext;
  private readonly IMapper _mapper;
  public ProfessorService(
      ILogger<ProfessorService> logger,
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

  public async Task<Result<IEnumerable<ResponseProfessorDto>, ProfessorErrors>> GetAsync(int Limit, string? Query)
  {
    if (Limit < 1)
    {
      _logger.LogWarning("A página deve ser número inteiro maior que zero.");
      return ProfessorErrors.InvalidFormat;
    }

    List<Professor> Professores = new List<Professor>();

    if (string.IsNullOrEmpty(Query))
    {
      Professores =
       await _dbContext.Professores
      //  .Skip((Page - 1) * Limit)
      //  .Take(Limit)
       .ToListAsync();
    }
    else
    {
      // professores =
      //    await _dbContext.Professores
      //    .Where(p => p.SearchVector.Matches(EF.Functions.ToTsQuery("portuguese", Query)))
      //    .Skip((Page - 1) * Limit)
      //    .Take(Limit)
      //    .ToListAsync();
    }

    IEnumerable<ResponseProfessorDto> response =
      Professores.Select(professor => _mapper.Map<ResponseProfessorDto>(professor));

    return response.ToList(); // TODO fix, ToList() should not be necessary, removing task seems to work, but WHY?
  }

  public async Task<Result<ResponseProfessorDto, ProfessorErrors>> FindAsync(int Id)
  {
    Professor? Professor = await _dbContext.Professores.FindAsync(Id);

    if (Professor is null)
    {
      _logger.LogWarning(
        "Professor com Id={Id}, não encontrado(a)",
        Id
      );
      return ProfessorErrors.RecordNotFound;
    }

    return _mapper.Map<ResponseProfessorDto>(Professor);
  }

  public async Task<Result<ResponseProfessorDto, ProfessorErrors>> PostAsync(CreateProfessorDto professorDto)
  {
    professorDto.CreatedBy =
      _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    Professor professor = _mapper.Map<Professor>(professorDto);

    // if (false) //TODO see if a Professor with the same Professor.CPF already exists
    // {
    //   _logger.LogWarning(
    //     "Erro Professor com CPF={CPF} já existe",
    //     Professor.Cpf
    //   );
    //   return ProfessorErrors.DuplicateEntry;
    // }

    _dbContext.Professores.Add(professor);
    await _dbContext.SaveChangesAsync();

    return _mapper.Map<ResponseProfessorDto>(professor);
  }

  public async Task<ProfessorErrors?> PutAsync(ResponseProfessorDto professorDto)
  {
    Professor? Professor =
      await _dbContext.Professores.FindAsync(professorDto.Id);

    if (Professor is null)
    {
      _logger.LogWarning(
        "Professor com Id={Id}, não encontrado(a)",
        professorDto.Id
      );
      return ProfessorErrors.RecordNotFound;
    }

    // Attach the entity to the context in the modified state
    _dbContext.Professores.Attach(Professor);

    Professor.Nome = professorDto.Nome;
    Professor.DataNascimento = professorDto.DataNascimento;
    Professor.Celular = professorDto.Celular;
    Professor.Cep = professorDto.Cep;
    Professor.Endereco = professorDto.Endereco;
    Professor.Numero = professorDto.Numero;
    Professor.Cpf = professorDto.Cpf;

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return null;
  }

  public async Task<ProfessorErrors?> DeleteAsync(int Id)
  {
    Professor? Professor =
    await _dbContext.Professores.FindAsync(Id);

    if (Professor is null)
    {
      _logger.LogWarning(
        "Professor com Id={Id}, não encontrado(a)",
        Id
      );
      return ProfessorErrors.RecordNotFound;
    }

    _dbContext.Professores.Remove(Professor);
    await _dbContext.SaveChangesAsync();

    return null;
  }
}