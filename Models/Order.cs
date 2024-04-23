using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Order
{
  public int OrderId { get; init; }
  [Required(ErrorMessage = "A venda é obrigatória")]
  public required Sale Sale { get; set; }
  [Required(ErrorMessage = "O status é obrigatório")]
  public required OrderStatus Status { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}