using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
  public int UserId { get; init; }
  [Required(ErrorMessage = "O role (a função do usuário) é obrigatório", AllowEmptyStrings = false)]
  public required string Role { get; set; }
  [EmailAddress(ErrorMessage = "E-mail inválido")]
  [Required(ErrorMessage = "Obrigatório preencher o e-mail", AllowEmptyStrings = false)]
  public required string Email { get; set; }
  // TODO add notification_preferences
  [Required(ErrorMessage = "O hash da senha é obrigatório")]
  public required byte[] PasswordHash { get; set; }
  [Required(ErrorMessage = "O Salt da senha é obrigatório")]
  public required byte[] PasswordSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
