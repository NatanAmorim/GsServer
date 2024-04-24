using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
  [Key]
  public Ulid UserId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "O role (a função do usuário) é obrigatório", AllowEmptyStrings = false)]
  public required string Role { get; set; }
  [EmailAddress(ErrorMessage = "E-mail inválido")]
  [Required(ErrorMessage = "Obrigatório preencher o e-mail", AllowEmptyStrings = false)]
  public required string Email { get; set; }
  // TODO add notification_preferences
  [Required]
  public required byte[] PasswordHash { get; set; }
  [Required]
  public required byte[] PasswordSalt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
