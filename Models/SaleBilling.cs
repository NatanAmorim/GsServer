using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SaleBilling
{
  public int SaleBillingId { get; init; }
  public required int SaleId { get; set; }
  [Length(4, 240, ErrorMessage = "O comentário deve ter entre 4 e 240 caracteres")]
  public required string Comments { get; set; }
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal TotalDiscount { get; init; }
  public required Payment Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
