using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class ProductVariant
{
  [Key]
  public Ulid ProductVariantId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "Obrigatório preencher a cor", AllowEmptyStrings = false)]
  public required string Color { get; set; }
  [Required(ErrorMessage = "Obrigatório preencher a cor", AllowEmptyStrings = false)]
  public required string Size { get; set; }
  [Required(ErrorMessage = "O código de barras é obrigatório", AllowEmptyStrings = true)]
  public required string BarCode { get; set; }
  [Required(ErrorMessage = "O SKU é obrigatório", AllowEmptyStrings = true)]
  public required string Sku { get; set; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O preço unitário não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O preço unitário é obrigatório")]
  public required decimal UnitPrice { get; set; }
  [Required(ErrorMessage = "O inventário é obrigatório")]
  public required ProductVariantInventory Inventory { get; set; }
}
