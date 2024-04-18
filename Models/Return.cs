using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class Return
{
  public int ReturnId { get; init; }
  [Column(TypeName = "decimal(19, 4)")]
  public decimal TotalAmountRefunded { get; set; }
  public required ICollection<ReturnItem> ItemsReturned { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
