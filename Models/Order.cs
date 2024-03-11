namespace gs_server.Models;

public class OrderModel
{
  public int OrderId { get; init; }
  public required SaleModel Sale { get; set; }
  public required OrderStatus Status { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public enum OrderStatus
{
  AwaitingPayment = 0,
  Pending = 1,
  Delivered = 2,
  Canceled = 3,
  Returned = 4,
}
