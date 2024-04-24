using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

/*
Implementation Considerations:
- Validity Period: Define start and end dates for promotions.
- Eligibility Criteria: Specify which users qualify for a particular promotion (new users, existing users, specific user segments).
- Stacking Rules: Decide whether promotions can be combined (e.g., using a discount code during a free trial).
- Communication Channels: Notify users about promotions via email, in-app messages, or push notifications.
*/
[Index(nameof(IsActive), IsUnique = false)]
public class Promotion // Represents special offers or discounts.
{
  [Key]
  public Ulid PromotionId { get; init; } = Ulid.NewUlid();
  [ForeignKey(nameof(CustomerId))]
  public Ulid CustomerId { get; init; }
  public virtual Customer Customer { get; set; } = null!;
  /// <summary>
  /// Name of the offer (e.g., "Summer Sale", "Introductory Discount", "Free Trials", "Referral Bonuses").
  /// </summary>
  [Required(ErrorMessage = "Obrigatório preencher o nome", AllowEmptyStrings = false)]
  public required string Name { get; set; }
  [Required(ErrorMessage = "A descrição é obrigatória", AllowEmptyStrings = true)]
  public required string Description { get; set; }
  /// <summary>
  /// Type of discount offered (e.g., "percentage", "fixed amount").
  /// </summary>
  [Required(ErrorMessage = "Obrigatório preencher o tipo de desconto", AllowEmptyStrings = false)]
  public required string DiscountType { get; set; }
  /// <summary>
  /// Date the offer becomes active.
  /// </summary>
  [Required(ErrorMessage = "A data de início é obrigatória")]
  public required DateOnly StartDate { get; set; }
  /// <summary>
  /// Date the offer expires (optional).
  /// </summary>
  [Required(ErrorMessage = "A data de fim é obrigatória")]
  public required DateOnly EndDate { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
