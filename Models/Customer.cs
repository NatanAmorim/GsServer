using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class Customer
{
  public int CustomerId { get; init; }
  public int? UserId { get; set; }
  public virtual User User { get; set; } = null!;
  public required Person Person { get; set; }
  public required ICollection<Person> Dependents { get; set; }
  [MinLength(4, ErrorMessage = "O Endereço deve ter no mínimo 4 caracteres")]
  [MaxLength(64, ErrorMessage = "O Endereço deve ter no máximo 64 caracteres")]
  public required string BillingAddress { get; set; }
  [MaxLength(240, ErrorMessage = "As Informações adicionais devem ter no máximo 240 caracteres")]
  public required string AdditionalInformation { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
