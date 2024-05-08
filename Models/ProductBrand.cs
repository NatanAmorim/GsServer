using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class ProductBrand
{
  [Key]
  public Ulid ProductBrandId { get; init; } = Ulid.NewUlid();
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string Name { get; set; }
}
