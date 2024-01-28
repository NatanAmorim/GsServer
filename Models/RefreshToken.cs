namespace gs_server.Models;

public class RefreshToken
{
  public int Id { get; init; }
  public required int UserId { get; init; }
  public required string Token { get; set; }
  public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
  public DateTime ExpiraEm { get; set; } = DateTime.UtcNow.AddDays(1);
  public bool IsValid { get; set; } = true;
}