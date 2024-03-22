namespace gs_server.Models;

public class SubscriptionModel
{
  public int SubscriptionId { get; init; }
  public required int DisciplineId { get; init; }
  public required int CustomerId { get; init; }
  public required int PayDay { get; set; }
  public required DateOnly StartDate { get; set; }
  // Date the subscription was cancelled
  public DateOnly? EndDate { get; set; }
  public required decimal Price { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
