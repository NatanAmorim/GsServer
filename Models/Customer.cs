using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class Customer
{
  public int CustomerId { get; init; }
  [ForeignKey(nameof(UserId))]
  public int UserId { get; set; }
  public virtual User User { get; set; } = null!;
  [Required(ErrorMessage = "A Pessoa é obrigatória")]
  public required Person Person { get; set; }
  [Required(ErrorMessage = "Os dependentes são obrigatórios")]
  public required ICollection<Person> Dependents { get; set; }
  [MinLength(4, ErrorMessage = "O Endereço deve ter no mínimo 4 caracteres")]
  [MaxLength(64, ErrorMessage = "O Endereço deve ter no máximo 64 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o endereço", AllowEmptyStrings = false)]
  public required string BillingAddress { get; set; }
  [MaxLength(240, ErrorMessage = "As Informações adicionais devem ter no máximo 240 caracteres")]
  [Required(ErrorMessage = "As informações adicionais são obrigatórias", AllowEmptyStrings = true)]
  public required string AdditionalInformation { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
