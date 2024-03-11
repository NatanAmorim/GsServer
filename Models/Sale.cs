namespace gs_server.Models;

public class SaleModel
{
  public int SaleId { get; init; }
  public CustomerModel? Customer { get; set; }
  public required string Observations { get; set; }
  public required List<SaleItemModel> Itens { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class SaleItemModel
{
  public int SaleItemId { get; init; }
  public required ProductVariantModel Product { get; set; }
  public required int Quantity { get; set; }
}
