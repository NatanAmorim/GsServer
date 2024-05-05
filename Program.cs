using System.Net;
using System.Net.Mime;
using System.Text;
using Amazon.CloudWatchLogs;
using GsServer.BackgroundServices;
using GsServer.Middlewares;
using GsServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.AwsCloudWatch;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<HostOptions>(options =>
{
  options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddAuthorization();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
// TODO fix
// builder.Services.AddHostedService<SubscriptionInvoiceBackgroundJob>();

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
app.MapGrpcService<AwsS3Service>();
app.MapGrpcService<CustomerRpcService>();
app.MapGrpcService<DisciplineRpcService>();
app.MapGrpcService<InstructorRpcService>();
app.MapGrpcService<NotificationRpcService>();
app.MapGrpcService<OrderRpcService>();
app.MapGrpcService<PaymentRpcService>();
app.MapGrpcService<ProductRpcService>();
app.MapGrpcService<PromotionRpcService>();
app.MapGrpcService<ReturnRpcService>();
app.MapGrpcService<SaleBillingRpcService>();
app.MapGrpcService<SaleRpcService>();
app.MapGrpcService<SubscriptionBillingRpcService>();
app.MapGrpcService<SubscriptionRpcService>();
app.MapGrpcService<UserRpcService>();

if (app.Environment.IsDevelopment())
{

  app.MapGrpcReflectionService();
  Log.Information(
    "Server running on port 7063"
  );
}

// Configure the HTTP request pipeline.
app.MapGet("/", () =>
  Results.Text(
    content: "<h1>Communication with gRPC endpoints must be made through a gRPC client.</h1>",
    contentType: MediaTypeNames.Text.Html,
    statusCode: StatusCodes.Status405MethodNotAllowed
  )
);

Log.Information("Server is ready to accept requests");

app.Use(async (context, next) =>
{
  // Middleware Log request details (e.g., method, path, headers)
  Log.Information(
    "Handling request ({TraceIdentifier}): {Method} {Path} {RequestIpAddress}",
    context.TraceIdentifier,
    context.Request.Method,
    context.Request.Path,
    context.Connection.RemoteIpAddress?.ToString() ?? string.Empty
  );

  // Proceed with handling the request
  await next();

  // Log response details (e.g., status code, headers)
  Log.Information(
    "Handled request ({TraceIdentifier})",
    context.TraceIdentifier
  );
});

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
