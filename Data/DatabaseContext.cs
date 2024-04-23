global using Microsoft.EntityFrameworkCore;
using GsServer.Models;

public class DatabaseContext(
    DbContextOptions<DatabaseContext> options,
    IConfiguration configuration
    ) : DbContext(options)
{
  private readonly IConfiguration _configuration = configuration;

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    base.OnConfiguring(optionsBuilder);
    /// This line should never be used in production and is only for debugging
    // optionsBuilder.LogTo(str => Debug.WriteLine(str));
    optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DB"));
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.UseSerialColumns();
    // modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
    // modelBuilder.ApplyConfiguration(new BackgroundJobConfiguration());
    // modelBuilder.ApplyConfiguration(new CustomerConfiguration());
    // modelBuilder.ApplyConfiguration(new DisciplineConfiguration());
    // modelBuilder.ApplyConfiguration(new InstructorConfiguration());
    // modelBuilder.ApplyConfiguration(new NotificationConfiguration());
    // modelBuilder.ApplyConfiguration(new OrderConfiguration());
    // modelBuilder.ApplyConfiguration(new PaymentConfiguration());
    // modelBuilder.ApplyConfiguration(new PersonConfiguration());
    // modelBuilder.ApplyConfiguration(new ProductConfiguration());
    // modelBuilder.ApplyConfiguration(new PromotionConfiguration());
    // modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    // modelBuilder.ApplyConfiguration(new ReturnConfiguration());
    // modelBuilder.ApplyConfiguration(new SaleConfiguration());
    // modelBuilder.ApplyConfiguration(new SaleBillingConfiguration());
    // modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
    // modelBuilder.ApplyConfiguration(new SubscriptionBillingConfiguration());
    // modelBuilder.ApplyConfiguration(new UserConfiguration());
  }

  public DbSet<Attendance> Attendances => Set<Attendance>();
  public DbSet<BackgroundJobStatus> BackgroundJobs => Set<BackgroundJobStatus>();
  public DbSet<Customer> Customers => Set<Customer>();
  public DbSet<Discipline> Disciplines => Set<Discipline>();
  public DbSet<Instructor> Instructors => Set<Instructor>();
  public DbSet<Notification> Notifications => Set<Notification>();
  public DbSet<Order> Orders => Set<Order>();
  public DbSet<Payment> Payments => Set<Payment>();
  public DbSet<Person> Persons => Set<Person>(); // Yes, it could also be called "People"
  public DbSet<Product> Products => Set<Product>();
  public DbSet<Promotion> Promotions => Set<Promotion>();
  public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
  public DbSet<Return> Returns => Set<Return>();
  public DbSet<Sale> Sales => Set<Sale>();
  public DbSet<SaleBilling> SaleBillings => Set<SaleBilling>();
  public DbSet<Subscription> Subscriptions => Set<Subscription>();
  public DbSet<SubscriptionBilling> SubscriptionBillings => Set<SubscriptionBilling>();
  public DbSet<User> Users => Set<User>();
}
