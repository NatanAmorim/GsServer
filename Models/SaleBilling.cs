using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SaleBilling
{
  [Key]
  public required Ulid SaleBillingId { get; init; }
  [ForeignKey(nameof(SaleId))]
  public required Ulid SaleId { get; set; }
  public virtual Sale Sale { get; set; } = null!;
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(AllowEmptyStrings = true)]
  public required string Observations { get; set; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O desconto total não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O desconto total é obrigatório")]
  public required decimal TotalDiscount { get; init; }
  [Required(ErrorMessage = "O pagamento é obrigatório")]
  public required Payment Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static SaleBilling FromProtoRequest(CreateSaleBillingRequest request, Ulid createdBy)
    => new()
    {
      SaleBillingId = Ulid.NewUlid(),
      SaleId = Ulid.Parse(request.SaleId),
      Observations = request.Observations,
      TotalDiscount = request.TotalDiscount,
      Payment = Payment.FromProtoRequest(request.Payment, createdBy),
      CreatedBy = createdBy,
    };

  public GetSaleBillingByIdResponse ToGetById()
    => new()
    {
      SaleBillingId = SaleBillingId.ToString(),
      Observations = Observations,
      TotalDiscount = TotalDiscount,
      Payment = Payment.ToGetById(),
    };
}
