namespace GsServer.Models;

public class ProductModel
{
  public int ProductId { get; init; }
  public required string Name { get; set; }
  public required string Description { get; set; }
  // Image path on a Cloud Storage (Like: Imgur, S3, Azure blob).
  // All images will be scaled to 128px(w) x 128px(h).
  public string? PicturePath { get; set; }
  public int? ProductBrandId { get; set; }
  // (e.g., Hats, Shirts, Pants, Shorts, Shoes, Dresses).
  public int? ProductCategoryId { get; set; }
  public required List<ProductVariantModel> Variants { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class ProductBrandModel
{
  public int ProductBrandId { get; init; }
  public required string Name { get; set; }
}

public class ProductCategoryModel
{
  public int ProductCategoryId { get; init; }
  public required string Name { get; set; }
}

public class ProductVariantModel
{
  public int ProductVariantId { get; init; }
  public required string Color { get; set; }
  public required string Size { get; set; }
  public required string BarCode { get; set; }
  public required string Sku { get; set; }
  public required decimal UnitPrice { get; set; }
  public required ProductVariantInventoryModel Inventory { get; set; }
}

public class ProductVariantInventoryModel
{
  public int ProductVariantInventoryId { get; init; }
  public int ProductVariantId { get; set; }
  public required int QuantityAvailable { get; set; }
  // The minimum number of units required to ensure no shortages will occur.
  // When the number of product units reaches this threshold level, a purchase
  // order must be done.
  public required int MinimumStockAmount { get; set; }
}

// public class ProductStockHistoryModel
// {
//   public int Id { get; init; }
//   public required int Amount { get; set; }
//   public required bool IsSale { get; set; }
//   public required bool IsReturnedItem { get; set; }
//   public required bool IsReplenishment { get; set; }
// }
