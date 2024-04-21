using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SubscriptionBilling
{
  public int SubscriptionBillingId { get; init; }
  public required int SubscriptionId { get; init; }
  public virtual Subscription Subscription { get; set; } = null!;
  [MinLength(4, ErrorMessage = "O comentário deve ter no mínimo 4 caracteres")]
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  public required string Comments { get; set; }
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal TotalDiscount { get; init; }
  public required Payment Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
