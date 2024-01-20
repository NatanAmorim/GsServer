using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gs_server.Dtos.Produtos;

public class CreateProdutoDto
{
  [Required]
  public required string Nome { get; set; }
  [Required]
  public required List<CreateProdutoVarianteDto> Variantes { get; set; }
  [JsonIgnore]
  public string CreatedBy { get; set; } = string.Empty;
}

public class CreateProdutoVarianteDto
{
  [Required]
  public required string Descricao { get; set; }
  [Required]
  public required string CodigoBarras { get; set; }
  [Required]
  public required double PrecoUnitario { get; set; }
  [Required]
  public required int EstoqueMinimo { get; set; }
  [Required]
  public required int Estoque { get; set; }
}