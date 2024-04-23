using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class ProductBrand
{
  public int ProductBrandId { get; init; }
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o nome da marca", AllowEmptyStrings = false)]
  public required string Name { get; set; }
}
