using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Customer
{
  public int CustomerId { get; init; }
  public int? UserId { get; set; }
  public virtual User User { get; set; } = null!;
  public required Person Person { get; set; }
  public required ICollection<Person> Dependents { get; set; }
  [Length(4, 64, ErrorMessage = "O Endereço deve ter entre 4 e 64 caracteres")]
  public required string BillingAddress { get; set; }
  [MaxLength(500)]
  public required string AdditionalInformation { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
