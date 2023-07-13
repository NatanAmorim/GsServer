using gs_server.Models.Produtos;

namespace gs_server.Models.Vendas;

public class Venda
{
  public required int Id { get; init; }
  public required string Nome { get; set; } //TODO Convert to enum
  public required List<VendaItem> Itens { get; set; }
  public List<VendaPagamento>? Pagamentos { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}


public class VendaItem
{
  public required int Id { get; init; }
  public required ProdutoVariante Produto { get; set; }
  public required int Quantidade { get; set; }
  public required double DescontoUnitario { get; set; }
  public required double DescontoTotal { get; set; }
}

public class VendaPagamento
{
  public required int Id { get; init; }
  public required bool IsPago { get; set; }
  public required double Valor { get; set; }
  public required string FormaPagamento { get; set; }
}