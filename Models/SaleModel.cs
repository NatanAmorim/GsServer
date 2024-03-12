namespace gs_server.Models;

public class SaleModel
{
  public int SaleId { get; init; }
  public CustomerModel? Customer { get; set; }
  public required string Observations { get; set; } // For details about returns, discounts and alike
  public required List<SaleItemModel> ItemsSold { get; set; }
  public List<ReturnModel> Returns { get; set; } = [];
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class SaleItemModel
{
  public int SaleItemId { get; init; }
  public required int ProductVariantId { get; set; }
  public required int QuantitySold { get; set; }
}

