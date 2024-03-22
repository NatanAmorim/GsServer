using System.Text;
using Amazon.CloudWatchLogs;
using GsServer.BackgroundServices;
using GsServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

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
          .GetBytes(
            builder.Configuration.GetSection(
              "Authentication:Schemes:Bearer:Secret"
            ).Value!
          )
      )
    };
  });


// TODO search how to AWS profile configured on my machine
string AwsAccessKeyId = builder.Configuration.GetSection("AWS:S3:AwsAccessKeyId").Value!;
string AwsSecretAccessKey = builder.Configuration.GetSection("AWS:S3:AwsSecretAccessKey").Value!;

// AWS CloudWatch client
// AmazonCloudWatchLogsClient client = new(AwsAccessKeyId, AwsSecretAccessKey);

builder.Host.UseSerilog(
  (context, configuration) =>
  {
    configuration.ReadFrom.Configuration(context.Configuration);
    // TODO
    // configuration.WriteTo.AmazonCloudWatch(
    //   // The name of the log group to log to
    //   logGroup: "/dotnet/gs-server/serilog",
    //   // A string that our log stream names should be prefixed with. We are just specifying the
    //   // start timestamp as the log stream prefix
    //   logStreamPrefix: DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"),
    //   // The AWS CloudWatch client to use
    //   cloudWatchClient: client
    // );
  }
);

WebApplication app = builder.Build();

app.MapGrpcService<AttendanceRpcService>();
app.MapGrpcService<AuthRpcService>();
app.MapGrpcService<CustomerRpcService>();
app.MapGrpcService<DisciplineRpcService>();
app.MapGrpcService<OrderRpcService>();
app.MapGrpcService<ProductRpcService>();
app.MapGrpcService<SaleRpcService>();
app.MapGrpcService<InstructorRpcService>();
app.MapGrpcService<UserRpcService>();

if (app.Environment.IsDevelopment())
{
  // Configure the HTTP request pipeline.
  app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
  app.MapGrpcReflectionService();
  Log.Debug(
    "Server initialized at port 7063"
  );
}
else
{
  Log.Information("Server initialized");
}

app.Use(async (context, next) =>
{
  // Middleware Log request details (e.g., method, path, headers)
  Log.Information(
    "({RequestIpAddress} - {TraceIdentifier}) Received gRPC request: {Method} {Path}",
    context.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
    context.TraceIdentifier,
    context.Request.Method,
    context.Request.Path
  );

  // Proceed with handling the request
  await next();

  // Log response details (e.g., status code, headers)
  Log.Information(
    "({TraceIdentifier}) Sent gRPC response",
    context.TraceIdentifier
  );
});

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
