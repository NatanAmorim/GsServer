namespace GsServer.Models;

public class BackgroundJobModel
{
  public int BackgroundJobId { get; init; }
  public required string Name { get; set; }
  public bool HasFinished { get; set; } = false;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}