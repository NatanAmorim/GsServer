global using Microsoft.EntityFrameworkCore;
using gs_server;
using gs_server.EntityConfigurations;
using gs_server.Models;
public class DatabaseContext(
    DbContextOptions<DatabaseContext> options,
    IConfiguration configuration
    ) : DbContext(options)
{
  private readonly IConfiguration _configuration = configuration;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
    optionsBuilder.UseNpgsql(_configuration.GetConnectionString("db"));
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseSerialColumns();
    modelBuilder.ApplyConfiguration(new AulaConfiguration());
    modelBuilder.ApplyConfiguration(new ClienteConfiguration());
    modelBuilder.ApplyConfiguration(new EncomendaConfiguration());
    modelBuilder.ApplyConfiguration(new EnderecoConfiguration());
    modelBuilder.ApplyConfiguration(new MensalidadeConfiguration());
    modelBuilder.ApplyConfiguration(new PessoaConfiguration());
    modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
    modelBuilder.ApplyConfiguration(new ProfessorConfiguration());
    modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
    modelBuilder.ApplyConfiguration(new VendaConfiguration());
  }

  public DbSet<Aula> Aulas => Set<Aula>();
  public DbSet<Cliente> Clientes => Set<Cliente>();
  public DbSet<Encomenda> Encomendas => Set<Encomenda>();
  public DbSet<Endereco> Enderecos => Set<Endereco>();
  public DbSet<Mensalidade> Mensalidades => Set<Mensalidade>();
  public DbSet<Pessoa> Pessoas => Set<Pessoa>();
  public DbSet<Produto> Produtos => Set<Produto>();
  public DbSet<Professor> Professores => Set<Professor>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
  public DbSet<Usuario> Usuarios => Set<Usuario>();
  public DbSet<Venda> Vendas => Set<Venda>();
}
