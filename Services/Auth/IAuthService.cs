using gs_server.Dtos.Auth;
using gs_server.Dtos.Usuarios;
using gs_server.Models.Usuarios;

namespace gs_server.Services.Auth;
public interface IAuthService
{
  public Task<ResponseLoginDto?> LoginAsync(RequestLoginDto loginDto);
  public Task<ResponseUsuarioDto?> PostAsync(CreateUsuarioDto usuarioDto);
  public Task<ResponseUsuarioDto?> PostFirstAsync(CreateUsuarioDto usuarioDto);
  public Task<bool> ChangePassword(string senhaAntiga, string senhaNova);
  public Task<string?> RefreshToken(string refreshToken);
}