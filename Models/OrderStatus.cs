namespace GsServer.Models;

public enum OrderStatus
{
  AwaitingPayment = 0,
  Pending = 1,
  Delivered = 2,
  Canceled = 3,
  Returned = 4,
}
