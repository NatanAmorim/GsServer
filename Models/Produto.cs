namespace gs_server.Models.Produtos;

public class Produto
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required List<ProdutoVariante> Variacoes { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class ProdutoVariante
{
  public required int Id { get; init; }
  public required string Descricao { get; set; }
  public required string CodigoBarras { get; set; }
  public required double PrecoUnitario { get; set; }
  public required int EstoqueMinimo { get; set; }
  public required int Estoque { get; set; }
}

public class ProdutoHistoricoEstoque
{
  public required int Id { get; init; }
  public required int Quantidade { get; set; }
  public required bool IsVenda { get; set; }
  public required bool IsRetornoItens { get; set; }
  public required bool IsReabastecimento { get; set; }
}