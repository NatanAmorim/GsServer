namespace gs_server.Models;

public class UserModel
{
  public int UserId { get; init; }
  public required string Role { get; set; }
  public required string Email { get; set; }
  // TODO add notification_preferences
  public required byte[] PasswordHash { get; set; }
  public required byte[] PasswordSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
