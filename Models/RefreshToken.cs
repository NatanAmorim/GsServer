using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(Token), nameof(IsValid), nameof(CreatedAt), AllDescending = true)] // Composite Index
public class RefreshToken
{
  public int RefreshTokenId { get; init; }
  [ForeignKey(nameof(UserId))]
  public required int UserId { get; init; }
  public virtual User User { get; set; } = null!;
  [MinLength(12, ErrorMessage = "O nome deve ter no mínimo 12 caracteres")]
  [Required(ErrorMessage = "O token é obrigatório", AllowEmptyStrings = false)]
  public required string Token { get; set; }
  public DateTime ExpiresIn { get; set; } = DateTime.UtcNow.AddDays(28);
  public bool IsExpired() => DateTime.UtcNow > ExpiresIn;
  public bool IsValid { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
