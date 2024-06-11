using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Return
{
  [Key]
  public required Ulid ReturnId { get; init; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "A quantia total reembolsada não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "A quantia total reembolsada é obrigatória")]
  public required decimal TotalAmountRefunded { get; set; }
  [Required(ErrorMessage = "Os itens retornados são obrigatórios")]
  public required ICollection<ReturnItem> ItemsReturned { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Return FromProtoRequest(CreateReturnRequest request, Ulid createdBy)
    => new()
    {
      ReturnId = Ulid.NewUlid(),
      TotalAmountRefunded = request.TotalAmountRefunded,
      ItemsReturned = request.ItemsReturned.Select(
            ItemReturned => new ReturnItem
            {
              ReturnItemId = Ulid.NewUlid(),
              ProductVariantId = Ulid.Parse(ItemReturned.ProductVariantId),
              UnitPrice = ItemReturned.UnitPrice,
              QuantityReturned = ItemReturned.QuantityReturned,
            }
          ).ToList(),
      CreatedBy = createdBy,
    };

  public GetReturnByIdResponse ToGetById()
    => new()
    {
      ReturnId = ReturnId.ToString(),
      TotalAmountRefunded = TotalAmountRefunded,
      ItemsReturned =
      {
        ItemsReturned.Select(
          ItemReturned => new Protobufs.ReturnItem
          {
            ProductVariantId = ItemReturned.ProductVariantId.ToString(),
            UnitPrice = ItemReturned.UnitPrice,
            QuantityReturned = ItemReturned.QuantityReturned,
          }
        ).ToList(),
      }
    };
}
