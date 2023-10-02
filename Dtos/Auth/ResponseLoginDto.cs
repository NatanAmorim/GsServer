namespace gs_server.Dtos.Auth;

/// <summary>
/// Login response object
/// </summary>
public class ResponseLoginDto
{
  public required string AcessToken { get; set; }
  public required string RefreshToken { get; set; }
}