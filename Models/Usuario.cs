namespace gs_server.Models;

public class Usuario
{
  public required int Id { get; init; }
  public required string Role { get; set; }
  public required string Email { get; set; }
  public required byte[] SenhaHash { get; set; }
  public required byte[] SenhaSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}