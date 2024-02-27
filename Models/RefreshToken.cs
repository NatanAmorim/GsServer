namespace gs_server.Models;

public class RefreshToken
{
  public int Id { get; init; }
  public required int UserId { get; init; }
  public required string Token { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime ExpiresIn { get; set; } = DateTime.UtcNow.AddDays(28);
  public bool IsValid { get; set; } = true;
}