using System.Security.Claims;
using AutoMapper;
using gs_server;
using gs_server.Dtos.Professores;
using gs_server.Models.Professores;
using sgd_cms.ControlFlow.Professores;

namespace sgd_cms.Services.Professores;

public class ProfessorService : IProfessorService
{
  private readonly ILogger<ProfessorService> _logger;
  private readonly IHttpContextAccessor? _httpContextAccessor;
  private readonly IMapper _mapper;
  public ProfessorService(
      ILogger<ProfessorService> logger,
      IHttpContextAccessor? httpContextAccessor,
      IMapper mapper
    )
  {
    _logger = logger;
    _httpContextAccessor = httpContextAccessor;
    _mapper = mapper;
  }

  public Task<Result<IEnumerable<ResponseProfessorDto>, ProfessorErrors?>> GetAsync(int Limit, string? Query)
  {
    throw new NotImplementedException();
  }

  public Task<Result<ResponseProfessorDto?, ProfessorErrors?>> FindAsync(int Id)
  {
    throw new NotImplementedException();
  }

  public async Task<Result<ResponseProfessorDto, ProfessorErrors?>> PostAsync(CreateProfessorDto professorDto)
  {
    professorDto.CreatedBy =
        _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    Professor professor = _mapper.Map<Professor>(professorDto);

    // _dbContext.Professors.Add(professor);
    // await _dbContext.SaveChangesAsync();

    if (true)
    {
      return ProfessorErrors.DuplicateEntry;
    }

    return _mapper.Map<ResponseProfessorDto>(professor);
  }

  public Task<ProfessorErrors?> PutAsync(ResponseProfessorDto professorDto)
  {
    throw new NotImplementedException();
  }

  public Task<ProfessorErrors?> DeleteAsync(int Id)
  {
    throw new NotImplementedException();
  }
}