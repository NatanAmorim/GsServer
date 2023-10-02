using System.ComponentModel.DataAnnotations;

namespace gs_server.Dtos.Auth;

public class RequestLoginDto
{
  // <summary>The user name for login</summary>
  /// <example>maria_eduarda@email.com</example>
  [Required]
  [MinLength(4)]
  public required string Email { get; set; }

  // <summary>The password for login in clear text</summary>
  /// <example>Senh@ForteEx3mpl0</example>
  [Required]
  [MinLength(8)]
  public required string Senha { get; set; }
}