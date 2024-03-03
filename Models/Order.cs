namespace gs_server.Models;

public class OrderModel
{
  public int Id { get; init; }
  public required SaleModel Sale { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
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

