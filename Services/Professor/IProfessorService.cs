using gs_server;
using gs_server.Dtos.Professores;
using Microsoft.AspNetCore.Mvc;
using gs_server.ControlFlow.Professores;

namespace gs_server.Services.Professores;

public interface IProfessorService
{
  public Task<Result<IEnumerable<ResponseProfessorDto>, ProfessorErrors>> GetAsync(int Limit, string? Query);
  public Task<Result<ResponseProfessorDto, ProfessorErrors>> FindAsync(int Id);
  public Task<Result<ResponseProfessorDto, ProfessorErrors>> PostAsync(CreateProfessorDto professorDto);
  public Task<ProfessorErrors?> PutAsync(ResponseProfessorDto professorDto);
  public Task<ProfessorErrors?> DeleteAsync(int Id);
}