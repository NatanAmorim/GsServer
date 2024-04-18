using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(MobilePhoneNumber), IsUnique = true)]
public class Person
{
  public int PersonId { get; init; }
  [Length(5, 55)]
  public required string Name { get; set; }
  [MaxLength(16)]
  public required string MobilePhoneNumber { get; set; }
  [Length(10, 10)]
  public required string BirthDate { get; set; }
  /// <summary>
  /// Cadastro de Pessoas Físicas (CPF)
  /// </summary>
  [Length(14, 14)]
  public required string Cpf { get; set; }
  // TODO
  /// <summary>
  /// Carteira de Identidade Nacional (CIN)
  /// </summary>
  // public required string Cin { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}