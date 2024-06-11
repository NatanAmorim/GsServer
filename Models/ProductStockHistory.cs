using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class ProductStockHistory
{
  [Key]
  public required Ulid ProductStockHistoryId { get; init; }
  [Required(ErrorMessage = "A quantidade alterada é obrigatória")]
  public required int AmountChanged { get; set; }
  /// <summary>
  /// (e.g., Returned, sold, restocked,...)
  /// </summary>
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string ChangeDescription { get; set; }
}
