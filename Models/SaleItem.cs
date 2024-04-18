using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class SaleItem
{
  public int SaleItemId { get; init; }
  public required int ProductVariantId { get; set; }
  public virtual ProductVariant ProductVariant { get; set; } = null!;
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal UnitPrice { get; set; }
  public required int QuantitySold { get; set; }
}
