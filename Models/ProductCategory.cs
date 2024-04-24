using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class ProductCategory
{
  [Key]
  public Ulid ProductCategoryId { get; init; } = Ulid.NewUlid();
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o nome da categoria", AllowEmptyStrings = false)]
  public required string Name { get; set; }
}
