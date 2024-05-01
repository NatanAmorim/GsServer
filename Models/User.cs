using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
  [Key]
  public Ulid UserId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "O role (a função do usuário) é obrigatório", AllowEmptyStrings = false)]
  public string Role { get; set; } = "customer";
  [EmailAddress(ErrorMessage = "E-mail inválido")]
  [Required(ErrorMessage = "Obrigatório preencher o e-mail", AllowEmptyStrings = false)]
  public required string Email { get; set; }
  [Required]
  public required byte[] PasswordHash { get; set; }
  [Required]
  public required byte[] PasswordSalt { get; set; }
  [Required]
  public bool HasAllowedEmailNotifications { get; set; } = false;
  [Required]
  public bool HasAllowedWhatsAppNotifications { get; set; } = false;
  [Required]
  public bool IsActive { get; set; } = false;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
