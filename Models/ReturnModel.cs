namespace GsServer.Models;

public class ReturnModel
{
  public int ReturnId { get; init; }
  public decimal TotalAmountRefunded { get; set; }
  public required List<ReturnItemModel> ItemsReturned { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class ReturnItemModel
{
  public int ReturnItemId { get; init; }
  public required int ProductVariantId { get; set; }
  public required int QuantityReturned { get; set; }
}