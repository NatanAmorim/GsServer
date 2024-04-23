using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class Sale
{
  public int SaleId { get; init; }
  [ForeignKey(nameof(CustomerId))]
  public int? CustomerId { get; set; }
  public virtual Customer Customer { get; set; } = null!;
  /// <summary>
  /// For details about returns, discounts and alike
  /// </summary>
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(ErrorMessage = "O comentário é obrigatório", AllowEmptyStrings = true)]
  public required string Comments { get; set; }
  [Required(ErrorMessage = "Os itens são obrigatórios")]
  public required ICollection<SaleItem> ItemsSold { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
