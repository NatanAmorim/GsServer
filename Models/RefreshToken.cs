namespace GsServer.Models;

[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
  public int RefreshTokenId { get; init; }
  public required int UserId { get; init; }
  public virtual User User { get; set; } = null!;
  // TODO use varchar or char
  public required string Token { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime ExpiresIn { get; set; } = DateTime.UtcNow.AddDays(28);
  public bool IsValid { get; set; } = true;
}
