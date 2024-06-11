using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Sale
{
  [Key]
  public required Ulid SaleId { get; init; }
  [ForeignKey(nameof(CustomerId))]
  public Ulid? CustomerId { get; set; }
  public virtual Customer Customer { get; set; } = null!;
  /// <summary>
  /// For details about returns, discounts and alike
  /// </summary>
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(AllowEmptyStrings = true)]
  public required string Observations { get; set; }
  [Required(ErrorMessage = "Os itens são obrigatórios")]
  public required ICollection<SaleItem> ItemsSold { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Sale FromProtoRequest(CreateSaleRequest request, Ulid createdBy)
    => new()
    {
      SaleId = Ulid.NewUlid(),
      CustomerId = Ulid.Parse(request.CustomerId),
      Observations = request.Observations,
      ItemsSold = request.ItemsSold.Select(
        ItemSold => new Models.SaleItem
        {
          SaleItemId = Ulid.NewUlid(),
          ProductVariantId = Ulid.Parse(ItemSold.ProductVariantId),
          UnitPrice = ItemSold.UnitPrice,
          QuantitySold = ItemSold.QuantitySold,
        }
      ).ToList(),
      CreatedBy = createdBy,
    };

  public GetSaleByIdResponse ToGetById()
    => new()
    {
      Observations = Observations,
      ItemsSold = {
        ItemsSold.Select(
          ItemSold => new Protobufs.SaleItem
          {
            ProductVariantId = ItemSold.ProductVariantId.ToString(),
            UnitPrice = ItemSold.UnitPrice,
            QuantitySold = ItemSold.QuantitySold,
          }
        ).ToList(),
      }
    };
}
