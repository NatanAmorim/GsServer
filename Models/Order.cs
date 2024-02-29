namespace gs_server.Models;

public class Order
{
  public int Id { get; init; }
  public required Sale Sale { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

// TODO these Status should be per item
public enum OrderStatus
{
  AwaitingPayment = 0, // Aguardando pagamento.
  Pending = 1, // Pendente.
  Delivered = 2, // Entregue.
  Canceled = 3, // Cancelado.
  Returned = 4, // Devolvido.
}

