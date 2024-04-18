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
  public required string Comments { get; set; }
  [Length(4, 240, ErrorMessage = "O comentário deve ter entre 4 e 240 caracteres")]
  public required ICollection<SaleItem> ItemsSold { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
