
using AutoMapper;
using gs_server.Dtos.Usuarios;
using gs_server.Models.Usuarios;

namespace gs_server.Services.Usuarios;

public class UsuarioService : IUsuarioService
{
  private readonly ILogger<UsuarioService> _logger;
  private readonly IHttpContextAccessor? _httpContextAccessor;
  private readonly DataBaseContext _dbContext;
  private readonly IMapper _mapper;
  public UsuarioService(
    ILogger<UsuarioService> logger,
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
  public async Task<int> CountAsync(string? Query)
  {
    // TODO: implement text search with query
    return await _dbContext.Usuarios.CountAsync();
  }

  public async Task<IEnumerable<ResponseLeanUsuarioDto>> GetAsync(int Page, int Limit, string? Query)
  {
    // TODO: implement text search with query
    List<Usuario> usuarios =
      await _dbContext.Usuarios
      .Skip((Page - 1) * Limit)
      .Take(Limit)
      .ToListAsync();

    return usuarios.Select(usuario => _mapper.Map<ResponseLeanUsuarioDto>(usuario));
  }

  public async Task<ResponseUsuarioDto?> FindAsync(int Id)
  {
    Usuario? usuario = await _dbContext.Usuarios.FindAsync(Id);

    if (usuario is null)
    {
      return null;
    }

    return _mapper.Map<ResponseUsuarioDto>(usuario);
  }

  public async Task<bool> PutAsync(ResponseUsuarioDto usuarioDto)
  {
    Usuario? usuario = await _dbContext.Usuarios.FindAsync(usuarioDto.Id);

    if (usuario is null)
    {
      return false;
    }

    // Attach the entity to the context in the modified state
    _dbContext.Usuarios.Attach(usuario);

    usuario.Email = usuarioDto.Email;
    usuario.Nome = usuarioDto.Nome;

    // Save the changes to the database
    await _dbContext.SaveChangesAsync();

    return true;
  }

  public async Task<bool> DeleteAsync(int Id)
  {
    Usuario? usuario = await _dbContext.Usuarios.FindAsync(Id);

    if (usuario is null)
    {
      return false;
    }

    _dbContext.Usuarios.Remove(usuario);
    await _dbContext.SaveChangesAsync();

    return true;
  }
}