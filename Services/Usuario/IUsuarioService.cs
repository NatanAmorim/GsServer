using gs_server.Dtos.Usuarios;

namespace gs_server.Services.Usuarios;

public interface IUsuarioService
{
  public Task<int> CountAsync(string? Query);
  public Task<IEnumerable<ResponseLeanUsuarioDto>> GetAsync(int Page, int Limit, string? Query);
  public Task<ResponseUsuarioDto?> FindAsync(int Id);
  public Task<bool> PutAsync(ResponseUsuarioDto usuarioDto);
  public Task<bool> DeleteAsync(int Id);
}