namespace gs_server;

public class Tuition // Mensalidade.
{
  public required int Id { get; init; }
  public required string PaymentDate { get; set; }
  public required double TotalPrice { get; set; }
  public required double AmountPaid { get; set; }
  public required double TotalDiscount { get; set; }
  public required string PaymentMethod { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}