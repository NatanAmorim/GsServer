namespace gs_server.Models;

public class ProductModel
{
  public int Id { get; init; }
  public required string Name { get; set; }
  public string? PicturePath { get; set; }
  public required List<ProductVariantModel> Variants { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class ProductVariantModel
{
  public int Id { get; init; }
  public required string Description { get; set; }
  public required string BarCode { get; set; }
  public required float UnitPrice { get; set; }
  public required int StockAmount { get; set; }
  public required int StockMinimumAmount { get; set; }
}

// public class ProductStockHistoryModel
// {
//   public int Id { get; init; }
//   public required int Amount { get; set; }
//   public required bool IsSale { get; set; }
//   public required bool IsReturnedItem { get; set; }
//   public required bool IsReplenishment { get; set; }
// }