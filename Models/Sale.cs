using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Sale
{
  public int SaleId { get; init; }
  public int? CustomerId { get; set; }
  public virtual Customer Customer { get; set; } = null!;
  /// <summary>
  /// For details about returns, discounts and alike
  /// </summary>
  [MinLength(4, ErrorMessage = "O comentário deve ter no mínimo 4 caracteres")]
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  public required string Comments { get; set; }
  public required ICollection<SaleItem> ItemsSold { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
