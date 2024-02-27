namespace gs_server.Models;

public class User
{
  public required int Id { get; init; }
  public required string Role { get; set; }
  public required string Email { get; set; }
  public required byte[] PasswordHash { get; set; }
  public required byte[] PasswordSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}