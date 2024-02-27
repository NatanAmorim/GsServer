namespace gs_server.Models;

public class Order
{
  public required int Id { get; init; }
  public required Customer Customer { get; set; }
  public required Sale Sale { get; set; }
  public required OrderStatus OrderStatus { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public enum OrderStatus
{
  AwaitingPayment = 0, // Aguardando pagamento.
  Pending = 1, // Pendente.
  Delivered = 2, // Entregue.
  Canceled = 3, // Cancelado.
  Returned = 4, // Devolvido.
}

