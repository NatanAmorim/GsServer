using System.ComponentModel.DataAnnotations;

namespace gs_server.Dtos.Produtos;

public class ResponseProdutoDto
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required List<ResponseProdutoVarianteDto> Variacoes { get; set; }
}

public class ResponseProdutoVarianteDto
{
  public required int Id { get; init; }
  public required string Descricao { get; set; }
  public required string CodigoBarras { get; set; }
  public required double PrecoUnitario { get; set; }
  public required int EstoqueMinimo { get; set; }
  public required int Estoque { get; set; }
}