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
    modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
    modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
    modelBuilder.ApplyConfiguration(new OrderConfiguration());
    modelBuilder.ApplyConfiguration(new PersonConfiguration());
    modelBuilder.ApplyConfiguration(new ProductConfiguration());
    modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    modelBuilder.ApplyConfiguration(new SaleConfiguration());
    modelBuilder.ApplyConfiguration(new InstructorConfiguration());
    modelBuilder.ApplyConfiguration(new UserConfiguration());
  }

  public DbSet<AttendanceModel> Attendances => Set<AttendanceModel>();
  public DbSet<BackgroundJobModel> BackgroundJobs => Set<BackgroundJobModel>();
  public DbSet<CustomerModel> Customers => Set<CustomerModel>();
  public DbSet<DisciplineModel> Disciplines => Set<DisciplineModel>();
  public DbSet<InstructorModel> Instructors => Set<InstructorModel>();
  public DbSet<NotificationModel> Notifications => Set<NotificationModel>();
  public DbSet<OrderModel> Orders => Set<OrderModel>();
  public DbSet<PaymentModel> Payments => Set<PaymentModel>();
  public DbSet<PersonModel> People => Set<PersonModel>();
  public DbSet<ProductModel> Products => Set<ProductModel>();
  public DbSet<PromotionModel> Promotions => Set<PromotionModel>();
  public DbSet<RefreshTokenModel> RefreshTokens => Set<RefreshTokenModel>();
  public DbSet<SaleModel> Sales => Set<SaleModel>();
  public DbSet<SaleBillingModel> SalesBilling => Set<SaleBillingModel>();
  public DbSet<SubscriptionModel> Subscriptions => Set<SubscriptionModel>();
  public DbSet<SubscriptionBillingModel> SubscriptionsBilling => Set<SubscriptionBillingModel>();
  public DbSet<UserModel> Users => Set<UserModel>();
}
