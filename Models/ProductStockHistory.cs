using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class ProductStockHistory
{
  public int ProductStockHistoryId { get; init; }
  [Required(ErrorMessage = "A quantidade alterada é obrigatória")]
  public required int AmountChanged { get; set; }
  /// <summary>
  /// (e.g., Returned, sold, restocked,...)
  /// </summary>
  [Required(ErrorMessage = "Obrigatório preencher a descrição da mudança", AllowEmptyStrings = false)]
  public required string ChangeDescription { get; set; }
}
