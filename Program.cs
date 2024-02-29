using System.Text;
using gs_server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddAuthorization();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

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
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(builder.Configuration.GetSection("Authentication:Schemes:Bearer:Secret").Value!)
      )
    };
  });

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Host.UseSerilog(
  (context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

// app.MapGrpcService<AttendanceRpcService>();
app.MapGrpcService<AuthRpcService>();
// app.MapGrpcService<CustomerService>();
// app.MapGrpcService<DisciplineService>();
// app.MapGrpcService<OrderService>();
app.MapGrpcService<ProductRpcService>();
// app.MapGrpcService<SaleService>();
// app.MapGrpcService<TeacherService>();
// app.MapGrpcService<TuitionService>();
// app.MapGrpcService<UserService>();
app.MapGrpcService<UserRpcService>();

if (app.Environment.IsDevelopment())
{
  app.MapGrpcReflectionService();
}

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
