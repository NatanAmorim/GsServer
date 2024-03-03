namespace gs_server;

public class TuitionModel
{
  public int Id { get; init; }
  public required string PaymentDate { get; set; }
  public required float TotalPrice { get; set; }
  public required float AmountPaid { get; set; }
  public required float TotalDiscount { get; set; }
  public required string PaymentMethod { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}