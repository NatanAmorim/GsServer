namespace gs_server.Models;

public class Sale
{
  public required int Id { get; init; }
  public string Description { get; set; } = string.Empty;
  public required double TotalPrice { get; set; }
  public required double AmountPaid { get; set; }
  public required double TotalDiscount { get; set; }
  public required List<SaleItem> Itens { get; set; }
  public List<SalePayment> Payments { get; set; } = [];
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class SaleItem
{
  public required int Id { get; init; }
  public required ProductVariant Product { get; set; }
  public required int AmountItemsSold { get; set; }
}

public class SalePayment
{
  public required int Id { get; init; }
  public required double AmountPaid { get; set; }
  public string PaymentMethod { get; set; } = string.Empty;
}
