namespace gs_server.Models;

public class Encomenda
{
  public required int Id { get; init; }
  public required EncomendaStatus Status { get; set; } //TODO Convert to enum
  public required Cliente Cliente { get; set; }
  public required List<EncomendaItem> Itens { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class EncomendaItem
{
  public required int Id { get; init; }
  public required int Quantidade { get; set; }
  public required ProdutoVariante Produto { get; set; }
}

public enum EncomendaStatus
{
  AguardandoPagamento = 0,
  Pendente = 1,
  Entregue = 2,
  Cancelado = 3,
  Devolvido = 4,
}

