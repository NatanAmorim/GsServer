using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Order
{
  [Key]
  public required Ulid OrderId { get; init; }
  [Required(ErrorMessage = "A venda é obrigatória")]
  public required Sale Sale { get; set; }
  [Required(ErrorMessage = "O status é obrigatório")]
  public required OrderStatus Status { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  /// TODO
  // public static Order FromProtoRequest(CreateOrderRequest request, Ulid createdBy)
  //   => new Order()
  // {
  //   PREENCHER
  // };

  /// TODO
  // public GetOrderByIdResponse ToGetById()
  //   => new GetOrderByIdResponse
  // {
  //   PREENCHER
  // };
}
