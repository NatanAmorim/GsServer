using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class Return
{
  public int ReturnId { get; init; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "A quantia total reembolsada não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "A quantia total reembolsada é obrigatória")]
  public required decimal TotalAmountRefunded { get; set; }
  [Required(ErrorMessage = "Os itens retornados são obrigatórios")]
  public required ICollection<ReturnItem> ItemsReturned { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
