using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class ReturnItem
{
  public int ReturnItemId { get; init; }
  public required int ProductVariantId { get; set; }
  public virtual ProductVariant Product { get; set; } = null!;
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal UnitPrice { get; set; }
  public required int QuantityReturned { get; set; }
}
