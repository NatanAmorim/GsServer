namespace gs_server.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SaleBillingModel
{
  public int SaleBillingId { get; init; }
  public required int SaleId { get; set; }
  public required string Observations { get; set; }
  public required decimal TotalDiscount { get; init; }
  public required PaymentModel Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
