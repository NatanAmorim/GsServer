using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(PayDay), nameof(IsActive))]
public class Subscription
{
  public int SubscriptionId { get; init; }
  public required int DisciplineId { get; init; }
  public virtual Discipline Discipline { get; set; } = null!;
  public required int CustomerId { get; init; }
  public virtual Customer Customer { get; set; } = null!;
  [Range(1, 28, MinimumIsExclusive = false, MaximumIsExclusive = false)]
  public required int PayDay { get; set; }
  /// <summary>
  /// Date the subscription has begin
  /// </summary>
  public required DateOnly StartDate { get; set; }
  /// <summary>
  /// Date the subscription was cancelled
  /// </summary>
  public DateOnly? EndDate { get; set; }
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal Price { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
