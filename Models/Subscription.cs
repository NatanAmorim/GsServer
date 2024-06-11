using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

[Index(nameof(PayDay), nameof(IsActive))]
public class Subscription
{
  [Key]
  public required Ulid SubscriptionId { get; init; }
  [ForeignKey(nameof(DisciplineId))]
  public required Ulid DisciplineId { get; init; }
  public virtual Discipline Discipline { get; set; } = null!;
  [Required(ErrorMessage = "O cliente é obrigatório")]
  [ForeignKey(nameof(CustomerId))]
  public required Ulid CustomerId { get; init; }
  public virtual Customer Customer { get; set; } = null!;
  [Range(1, 28, ErrorMessage = "O dia de pagamento deve ser entre 1 e 28")]
  [Required(ErrorMessage = "O dia de pagamento é obrigatório")]
  public required int PayDay { get; set; }
  /// <summary>
  /// Date the subscription has begin
  /// </summary>
  [Required(ErrorMessage = "A data de inicio inscrição é obrigatória")]
  public required DateOnly StartDate { get; set; }
  /// <summary>
  /// Date the subscription was cancelled
  /// </summary>
  public DateOnly? EndDate { get; set; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O preço não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O preço é obrigatório")]
  public required decimal Price { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Subscription FromProtoRequest(CreateSubscriptionRequest request, Ulid createdBy)
    => new()
    {
      SubscriptionId = Ulid.NewUlid(),
      DisciplineId = Ulid.Parse(request.DisciplineId),
      CustomerId = Ulid.Parse(request.CustomerId),
      PayDay = request.PayDay,
      StartDate = request.StartDate,
      Price = request.Price,
      CreatedBy = createdBy,
    };

  public GetSubscriptionByIdResponse ToGetById()
    => new()
    {
      Discipline = Discipline.ToGetById(),
      Customer = Customer.ToGetById(),
      PayDay = PayDay,
      StartDate = StartDate,
      Price = Price,
    };
}
