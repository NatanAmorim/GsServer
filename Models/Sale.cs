namespace gs_server.Models;

public class SaleModel
{
  public int Id { get; init; }
  public CustomerModel? Customer { get; set; }
  public required string Observations { get; set; }
  public required float TotalPrice { get; set; }
  public required float AmountPaid { get; set; }
  public required float TotalDiscount { get; set; }
  public required List<SaleItemModel> Itens { get; set; }
  public List<SalePaymentModel> Payments { get; set; } = [];
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public class SaleItemModel
{
  public int Id { get; init; }
  public required ProductVariantModel Product { get; set; }
  public required int AmountItemsSold { get; set; }
}

public class SalePaymentModel
{
  public int Id { get; init; }
  public required float AmountPaid { get; set; }
  public required string PaymentMethod { get; set; }
}
