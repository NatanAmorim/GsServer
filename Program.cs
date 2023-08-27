using System.Reflection;
using System.Text;
using gs_server;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using sgd_cms.Services.Professores;
using Swashbuckle.AspNetCore.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProfessorService, ProfessorService>();

builder.Services.AddCors(options =>
{
  options.AddDefaultPolicy(
      policy =>
      {
        policy.WithOrigins(
          // TODO replace placeholder origins when deploying
          "https://microsoft.com",
          "https://outlook.live.com"
        )
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "DELETE");
      });

  // This is for local development only
  options.AddPolicy("AllowAll",
          builder =>
          {
            builder
              .AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
          });
});

builder.Services.AddControllers();
// builder.Services.AddDbContext<DataContext>(options =>
//   options.UseNpgsql(
//     builder.Configuration.GetConnectionString("Sql")
//   )
// );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "SGD-CMS API",
    Description = "An example description",
    // TermsOfService = new Uri("https://example.com/terms"),
    // Contact = new OpenApiContact
    // {
    //   Name = "Example Contact",
    //   Url = new Uri("https://example.com/contact")
    // },
    // License = new OpenApiLicense
    // {
    //   Name = "Example License",
    //   Url = new Uri("https://example.com/license")
    // }
  });

  options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
  {
    Description = "Default authorization using Bearer schema (\"bearer {token}\")",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey
  });

  options.OperationFilter<SecurityRequirementsOperationFilter>();

  // This is used to generate swagger docs in XML
  // More info in: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio-code
  String xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidIssuer = builder.Configuration.GetSection("Authentication:Schemes:Bearer:Issuer").Value!,
        // TODO remover audiencia
        // ValidAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:Audiences").Get<List<string>>(),

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

builder.Services.AddRateLimiter(options =>
{
  options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
  app.UseCors("AllowAll");
}
else
{
  app.UseCors();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.UseRateLimiter();

app.Run();
