namespace gs_server.Models;

public class Product
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public string? Picture { get; set; }
  public required List<ProductVariant> Variants { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class ProductVariant
{
  public required int Id { get; init; }
  public required string Description { get; set; }
  public required string BarCode { get; set; }
  public required double UnitPrice { get; set; }
  public required int StockAmount { get; set; }
  public required int StockMinimumAmount { get; set; }
}

// TODO track changes in a ephemeral table
// public class ProductStockHistory
// {
//   public required int Id { get; init; }
//   public required int Amount { get; set; }
//   public required bool IsSale { get; set; }
//   public required bool IsReturnedItem { get; set; }
//   public required bool IsReplenishment { get; set; }
// }