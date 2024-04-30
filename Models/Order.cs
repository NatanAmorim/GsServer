using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Order
{
  [Key]
  public Ulid OrderId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "A venda é obrigatória")]
  public required Sale Sale { get; set; }
  [Required(ErrorMessage = "O status é obrigatório")]
  public required OrderStatus Status { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public Ulid? CreatedBy { get; set; }
}
