using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class ProductVariant
{
  public int ProductVariantId { get; init; }
  public required string Color { get; set; }
  public required string Size { get; set; }
  public required string BarCode { get; set; }
  public required string Sku { get; set; }
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal UnitPrice { get; set; }
  public required ProductVariantInventory Inventory { get; set; }
}
