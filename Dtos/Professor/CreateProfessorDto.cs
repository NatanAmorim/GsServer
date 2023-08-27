using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gs_server.Dtos.Professores;

/// <summary>
/// Created professor object
/// </summary>
public class CreateProfessorDto
{
  /// <example>Maria Clara</example>
  [Required]
  public required string Nome { get; set; }
  /// <example>25/04/2004</example>
  public required string DataNascimento { get; set; }
  /// <example>(18) 99999-9999</example>
  public required string Celular { get; set; }
  /// <example>99.999-999</example>
  public required string Cep { get; set; }
  /// <example>Rua Bandeirantes</example>
  public required string Endereco { get; set; }
  /// <example>540</example>
  public required string Numero { get; set; }
  /// <example>999.999.999-55</example>
  public required string Cpf { get; set; }
  [JsonIgnore]
  public string CreatedBy { get; set; } = string.Empty;
}