namespace gs_server;

public class Mensalidade
{
  public required int Id { get; init; }
  public required string DataPagamento { get; set; }
  public required double ValorPreco { get; set; }
  public required double ValorPago { get; set; }
  public required double DescontoTotal { get; set; }
  public required string FormaPagamento { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}