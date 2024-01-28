namespace gs_server.Models;

public class Professor
{
  public required int Id { get; init; }
  public required Pessoa Pessoa { get; set; }
  public required Usuario Usuario { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}