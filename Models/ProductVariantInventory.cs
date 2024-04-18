namespace GsServer.Models;

public class ProductVariantInventory
{
  public int ProductVariantInventoryId { get; init; }
  public int ProductVariantId { get; set; }
  public virtual ProductVariant ProductVariant { get; set; } = null!;
  public required int QuantityAvailable { get; set; }
  // The minimum number of units required to ensure no shortages will occur.
  // When the number of product units reaches this threshold level, a purchase
  // order must be done.
  public required int MinimumStockAmount { get; set; }
}
