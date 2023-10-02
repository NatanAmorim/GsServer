global using Microsoft.EntityFrameworkCore;
using gs_server.Models.Professores;
using gs_server.Models.RefreshTokens;
using gs_server.Models.Usuarios;
public class DataBaseContext : DbContext
{
  private readonly IConfiguration _configuration;

  public DataBaseContext(
      DbContextOptions<DataBaseContext> options,
      IConfiguration configuration
    ) : base(options)
  {
    _configuration = configuration;
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseNpgsql(_configuration.GetConnectionString("db"));
  }

  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
  public DbSet<Professor> Professores => Set<Professor>();
  public DbSet<Usuario> Usuarios => Set<Usuario>();
}
