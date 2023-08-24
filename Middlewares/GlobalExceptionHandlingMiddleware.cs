using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace gs_server;

/// <summary>
/// This is a Middleware for unexpected errors.
/// </summary>
// Exceptions are ONLY for exceptional circumtances, they are good use for I/O
// errors, don't use exceptions for control flow, because exceptions are slow.
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
  private readonly ILogger _logger;
  public GlobalExceptionHandlingMiddleware(ILogger logger)
  {
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context, RequestDelegate next)
  {
    try
    {
      await next(context);
    }
    catch (Exception exception)
    {
      _logger.LogError(exception, exception.Message);

      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

      ProblemDetails details = new()
      {
        Status = (int)HttpStatusCode.InternalServerError,
        Type = "Internal Server Errorr",
        Title = "An internal server error has accurred",
        Detail = "Something went wrong on our end. We apologize for the inconvenience, our team has been notified and is working to fix the issue. Please try again later.",
      };

      // string json = JsonSerializer.Serialize(details);
      // await context.Response.WriteAsync(json);
      await context.Response.WriteAsJsonAsync(details);
      context.Response.ContentType = "application/json";
    }
  }
}