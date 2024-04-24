using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class ProductVariantInventory
{
  [Key]
  public Ulid ProductVariantInventoryId { get; init; } = Ulid.NewUlid();
  [ForeignKey(nameof(ProductVariantId))]
  public Ulid ProductVariantId { get; set; }
  public virtual ProductVariant ProductVariant { get; set; } = null!;
  [Required(ErrorMessage = "A quantidade disponível é obrigatória")]
  public required int QuantityAvailable { get; set; }
  // The minimum number of units required to ensure no shortages will occur.
  // When the number of product units reaches this threshold level, a purchase
  // order must be done.
  [Required(ErrorMessage = "A quantidade mínima de estoque é obrigatória")]
  public required int MinimumStockAmount { get; set; }
}
