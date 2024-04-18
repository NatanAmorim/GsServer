using System.ComponentModel.DataAnnotations;

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
  public int PromotionId { get; init; }
  public int CustomerId { get; init; }
  public virtual Customer Customer { get; set; } = null!;
  // Name of the offer (e.g., "Summer Sale", "Introductory Discount", "Free Trials", "Referral Bonuses").
  public required string Name { get; set; }
  public required string Description { get; set; }
  // Type of discount offered (e.g., "percentage", "fixed amount").
  public required string DiscountType { get; set; }
  // Date the offer becomes active.
  public required DateOnly StartDate { get; set; }
  // Date the offer expires (optional).
  public required DateOnly EndDate { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
