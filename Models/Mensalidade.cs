namespace gs_server.Models.Mensalidades;

public class Mensalidade
{
  public required int Id { get; init; }
  public required bool IsPago { get; set; } //TODO Convert to enum
  public required string DataVencimento { get; set; }
  public required double Valor { get; set; }
  public required string FormaPagamento { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}