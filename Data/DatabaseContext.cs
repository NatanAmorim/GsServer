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
    modelBuilder.ApplyConfiguration(new AddressConfiguration());
    modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
    modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
    modelBuilder.ApplyConfiguration(new OrderConfiguration());
    modelBuilder.ApplyConfiguration(new PersonConfiguration());
    modelBuilder.ApplyConfiguration(new ProductConfiguration());
    modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    modelBuilder.ApplyConfiguration(new SaleConfiguration());
    modelBuilder.ApplyConfiguration(new TeacherConfiguration());
    modelBuilder.ApplyConfiguration(new TuitionConfiguration());
    modelBuilder.ApplyConfiguration(new UserConfiguration());
  }

  public DbSet<Discipline> Disciplines => Set<Discipline>();
  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<Address> Addresses => Set<Address>();
  public DbSet<Tuition> Tuitions => Set<Tuition>();
  public DbSet<Person> Person => Set<Person>();
  public DbSet<Product> Product => Set<Product>();
  public DbSet<Teacher> Teachers => Set<Teacher>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
  public DbSet<User> Users => Set<User>();
  public DbSet<Sale> Sales => Set<Sale>();
}
