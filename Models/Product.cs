using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Product
{
  public int ProductId { get; init; }
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [MaxLength(32, ErrorMessage = "O nome deve ter no máximo 32 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o nome", AllowEmptyStrings = false)]
  public required string Name { get; set; }
  // Image path on a Cloud Storage (Like: Imgur, S3, Azure blob).
  // All images will be scaled to 128px(w) x 128px(h).
  public string? PicturePath { get; set; }
  [Required(ErrorMessage = "A marca do produto é obrigatória")]
  public required ProductBrand ProductBrand { get; set; }
  // (e.g., Hats, Shirts, Pants, Shorts, Shoes, Dresses).
  [Required(ErrorMessage = "A categoria do produto é obrigatória")]
  public required ProductCategory ProductCategory { get; set; }
  [Required(ErrorMessage = "As variantes do produto são obrigatórias")]
  public required ICollection<ProductVariant> Variants { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
