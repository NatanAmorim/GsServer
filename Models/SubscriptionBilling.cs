namespace gs_server.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SubscriptionBillingModel
{
  public int SubscriptionBillingId { get; init; }
  public required int SubscriptionId { get; init; }
  public required string Observations { get; set; }
  public required float TotalDiscount { get; init; }
  public required PaymentModel Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
