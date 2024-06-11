using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

/// <summary>
/// Billing: Refers to the process of stating the amount due to the customer, it
/// is used for immediate payment situations and may not include detailed client
/// information (unlike an invoice).
/// </summary>
public class SubscriptionBilling
{
  [Key]
  public required Ulid SubscriptionBillingId { get; init; }
  [ForeignKey(nameof(SubscriptionId))]
  public required Ulid SubscriptionId { get; init; }
  public virtual Subscription Subscription { get; set; } = null!;
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(AllowEmptyStrings = true)]
  public required string Observations { get; set; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "A desconto total não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O desconto total é obrigatório")]
  public required decimal TotalDiscount { get; init; }
  [Required(ErrorMessage = "O pagamento é obrigatório")]
  public required Payment Payment { get; init; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static SubscriptionBilling FromProtoRequest(CreateSubscriptionBillingRequest request, Ulid createdBy)
    => new()
    {
      SubscriptionBillingId = Ulid.NewUlid(),
      SubscriptionId = Ulid.Parse(request.SubscriptionId),
      Observations = request.Observations,
      TotalDiscount = request.TotalDiscount,
      Payment = Payment.FromProtoRequest(request.Payment, createdBy),
      CreatedBy = createdBy,
    };

  public GetSubscriptionBillingByIdResponse ToGetById()
    => new()
    {
      SubscriptionBillingId = SubscriptionBillingId.ToString(),
      Observations = Observations,
      TotalDiscount = TotalDiscount,
      Payment = Payment.ToGetById(),
    };
}
