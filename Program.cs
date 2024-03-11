using System.Text;
using gs_server.BackgroundServices;
using gs_server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<HostOptions>(options =>
{
  options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddAuthorization();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddHostedService<SubscriptionInvoiceBackgroundJob>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateAudience = false,
      ValidateIssuer = true,
      ValidIssuer = builder.Configuration.GetSection("Authentication:Schemes:Bearer:Issuer").Value!,
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8
          .GetBytes(builder.Configuration.GetSection("Authentication:Schemes:Bearer:Secret").Value!)
        )
    };
  });

builder.Host.UseSerilog(
  (context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.MapGrpcService<AttendanceRpcService>();
app.MapGrpcService<AuthRpcService>();
app.MapGrpcService<CustomerRpcService>();
app.MapGrpcService<DisciplineRpcService>();
app.MapGrpcService<OrderRpcService>();
// app.MapGrpcService<ProductRpcService>();
app.MapGrpcService<SaleRpcService>();
app.MapGrpcService<InstructorRpcService>();
app.MapGrpcService<UserRpcService>();

if (app.Environment.IsDevelopment())
{
  app.MapGrpcReflectionService();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
