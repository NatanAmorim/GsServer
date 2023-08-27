using gs_server;
using gs_server.Dtos.Professores;
using sgd_cms.ControlFlow.Professores;

namespace sgd_cms.Services.Professores;

public interface IProfessorService
{
  // Result<ResponseProfessorDto?, ProfessorErrors?>
  public Task<Result<IEnumerable<ResponseProfessorDto>, ProfessorErrors?>> GetAsync(int Limit, string? Query);
  public Task<Result<ResponseProfessorDto?, ProfessorErrors?>> FindAsync(int Id);
  public Task<Result<ResponseProfessorDto, ProfessorErrors?>> PostAsync(CreateProfessorDto professorDto);
  public Task<ProfessorErrors?> PutAsync(ResponseProfessorDto professorDto);
  public Task<ProfessorErrors?> DeleteAsync(int Id);
}