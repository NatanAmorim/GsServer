using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class ReturnItem
{
  [Key]
  public Ulid ReturnItemId { get; init; } = Ulid.NewUlid();
  [ForeignKey(nameof(ProductVariantId))]
  public required Ulid ProductVariantId { get; set; }
  public virtual ProductVariant ProductVariant { get; set; } = null!;
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O preço unitário não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O preço unitário é obrigatório")]
  public required decimal UnitPrice { get; set; }
  [Required(ErrorMessage = "A quantidade retornada é obrigatória")]
  public required int QuantityReturned { get; set; }
}
