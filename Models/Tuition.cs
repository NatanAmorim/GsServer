namespace gs_server;

public class Tuition
{
  public int Id { get; init; }
  public required string PaymentDate { get; set; }
  public required float TotalPrice { get; set; }
  public required float AmountPaid { get; set; }
  public required float TotalDiscount { get; set; }
  public required string PaymentMethod { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}