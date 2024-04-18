using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
  public int UserId { get; init; }
  [Required(ErrorMessage = "O role (a função) é obrigatório", AllowEmptyStrings = false)]
  public required string Role { get; set; }
  [Required(ErrorMessage = "O e-mail é obrigatório", AllowEmptyStrings = false)]
  [EmailAddress]
  public required string Email { get; set; }
  // TODO add notification_preferences
  public required byte[] PasswordHash { get; set; }
  public required byte[] PasswordSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
