using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(MobilePhoneNumber), IsUnique = true)]
[Index(nameof(Cpf), IsUnique = true)]
public class Person
{
  public int PersonId { get; init; }
  [MinLength(5, ErrorMessage = "O nome completo deve ter no mínimo 5 caracteres")]
  [MaxLength(55, ErrorMessage = "O nome completo deve ter no máximo 55 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o nome completo", AllowEmptyStrings = false)]
  public required string FullName { get; set; }
  [MinLength(15, ErrorMessage = "O número de celular deve ter no mínimo 15 caracteres")]
  [MaxLength(16, ErrorMessage = "O número de celular deve ter no máximo 16 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o número de celular", AllowEmptyStrings = false)]
  public required string MobilePhoneNumber { get; set; }
  [Column(TypeName = "char(10)")]
  [MinLength(10, ErrorMessage = "A data de nascimento deve ter 10 caracteres")]
  [MaxLength(10, ErrorMessage = "A data de nascimento deve ter 10 caracteres")]
  [Required(ErrorMessage = "A data de nascimento é obrigatória", AllowEmptyStrings = true)]
  public required string BirthDate { get; set; }
  /// <summary>
  /// Cadastro de Pessoas Físicas (CPF)
  /// </summary>
  [Column(TypeName = "char(14)")]
  [MinLength(14, ErrorMessage = "O CPF deve ter 14 caracteres")]
  [MaxLength(14, ErrorMessage = "O CPF deve ter 14 caracteres")]
  [Required(ErrorMessage = "O CPF é obrigatório", AllowEmptyStrings = true)]
  public required string Cpf { get; set; }
  // TODO
  /// <summary>
  /// Carteira de Identidade Nacional (CIN)
  /// </summary>
  // [Required(ErrorMessage = "O CIN é obrigatório")]
  // public required string Cin { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}