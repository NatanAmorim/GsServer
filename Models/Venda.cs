namespace gs_server.Models;

public class Venda
{
  public required int Id { get; init; }
  public string Descricao { get; set; } = string.Empty;
  public required double ValorTotal { get; set; }
  public required double DescontoTotal { get; set; }
  public required List<VendaItem> Itens { get; set; }
  public List<VendaPagamento> Pagamentos { get; set; } = [];
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class VendaItem
{
  public required int Id { get; init; }
  public required ProdutoVariante Produto { get; set; }
  public required int Quantidade { get; set; }
}

public class VendaPagamento
{
  public required int Id { get; init; }
  public required bool IsPago { get; set; }
  public required double Valor { get; set; }
  public string FormaPagamento { get; set; } = string.Empty;
}
