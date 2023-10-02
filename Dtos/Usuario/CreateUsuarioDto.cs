using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace gs_server.Dtos.Usuarios;

/// <summary>
/// Created user object
/// </summary>
public class CreateUsuarioDto
{
  // <summary>email of the user</summary>
  /// <example>maria_eduarda@email.com</example>
  [Required]
  [MinLength(4)]
  public required string Email { get; set; }

  // <summary>email of the user</summary>
  /// <example>Senh@ForteEx3mpl0</example>
  [Required]
  [MinLength(8)]
  public required string Senha { get; set; }

  /// <summary>name of the user</summary>
  /// <example>José da Silva</example>
  [Required]
  [MinLength(4)]
  [MaxLength(140)]
  public required string Nome { get; set; }

  [JsonIgnore]
  public string CreatedBy { get; set; } = string.Empty;
}