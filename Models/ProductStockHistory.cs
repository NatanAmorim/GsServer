namespace GsServer.Models;

public class ProductStockHistory
{
  public int Id { get; init; }
  public required int AmountChanged { get; set; }
  public required string ChangeDescription { get; set; } // (e.g., Returned, sold, restocked,...)
}
