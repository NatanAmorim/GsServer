using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GsServer.Middlewares;

/// <summary>
/// This is a Middleware for unexpected errors.
/// </summary>
// Exceptions are ONLY for exceptional circumstances, they are good use for I/O
// errors, don't use exceptions for control flow, because exceptions are slow.
public class GlobalExceptionHandler(
  ILogger<GlobalExceptionHandler> logger
  ) : IExceptionHandler
{
  private readonly ILogger<GlobalExceptionHandler> _logger = logger;
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken
  )
  {
    _logger.LogError(exception, "Unknown Internal Server Error");

    /// Problem Details for HTTP APIs (Request for Comments: 7807), it proposes
    /// a standardized way to communicate errors in HTTP APIs, instead of just
    /// sending a generic error code, it can use a specific format outlined in
    /// RFC 7807 to provide more details about the problem. This helps
    /// developers using the API understand what went wrong and how to fix it.
    /// https://datatracker.ietf.org/doc/html/rfc7807

    ProblemDetails details = new()
    {
      Status = StatusCodes.Status500InternalServerError,
      Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
      Title = "Internal Server Error",
      /// The following message translates to:
      /// "Something went wrong on our end. We apologize for the inconvenience,
      /// our team has been notified and is working to fix the issue. Please try
      /// again later."
      Detail = "Algo deu errado do nosso lado. Pedimos desculpas pelo transtorno, nossa equipe foi notificada e está trabalhando para solucionar o problema. Por favor, tente novamente mais tarde.",
    };

    httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
    httpContext.Response.ContentType = "application/json";
    await httpContext.Response.WriteAsJsonAsync(details, cancellationToken: cancellationToken);

    /// TODO Is there a way of doing RFC 7807 with gRPC? something like
    // throw new RpcException(new Status(
    //   StatusCode.Internal, $"Algo deu errado do nosso lado. Pedimos desculpas pelo transtorno, nossa equipe foi notificada e está trabalhando para solucionar o problema. Por favor, tente novamente mais tarde."
    // ));

    return true;
  }
}