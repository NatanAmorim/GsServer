using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Owned]
public class SaleItem
{
  [Key]
  public required Ulid SaleItemId { get; init; }
  [ForeignKey(nameof(ProductVariantId))]
  public required Ulid ProductVariantId { get; set; }
  public virtual ProductVariant ProductVariant { get; set; } = null!;
  [Column(TypeName = "decimal(8, 4)")]
  [Range(0, 999_999.99, ErrorMessage = "O preço unitário não deve ser menos que R$ 0,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O preço unitário é obrigatório")]
  public required decimal UnitPrice { get; set; }
  public required int QuantitySold { get; set; }
}
