using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

// Sends reminders for upcoming events, invoice expirations, etc.
[Index(nameof(CreatedAt), nameof(IsUnread), IsUnique = false)]
public class Notification
{
  public int NotificationId { get; init; }
  public int UserId { get; init; }
  public virtual User User { get; set; } = null!;
  [Length(4, 12, ErrorMessage = "O título deve ter entre 4 e 12 caracteres")]
  public required string Title { get; set; }
  [Length(4, 120, ErrorMessage = "O título deve ter entre 4 e 64 caracteres")]
  public required string Message { get; set; }
  public bool IsUnread { get; set; } = true;
  public bool IsRead() => !IsUnread;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}