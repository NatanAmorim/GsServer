namespace gs_server.Models;

public class Teacher
{
  public int Id { get; init; }
  public required Person Person { get; set; }
  public required User User { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}