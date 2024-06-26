using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

// Sends reminders for upcoming events, invoice expirations, etc.
[Index(nameof(CreatedAt), IsUnique = false)]
[Index(nameof(IsUnread), IsUnique = false)]
public class Notification
{
  [Key]
  public required Ulid NotificationId { get; init; }
  [ForeignKey(nameof(UserId))]
  public Ulid UserId { get; init; }
  public virtual User User { get; set; } = null!;
  [MinLength(4, ErrorMessage = "O título deve ter no mínimo 4 caracteres")]
  [MaxLength(16, ErrorMessage = "O título deve ter no máximo 16 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string Title { get; set; }
  [MinLength(16, ErrorMessage = "A mensagem deve ter no mínimo 16 caracteres")]
  [MaxLength(120, ErrorMessage = "A mensagem deve ter no máximo 120 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string Message { get; set; }
  public bool IsUnread { get; set; } = true;
  public bool IsRead() => !IsUnread;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Notification FromProtoRequest(CreateNotificationRequest request, Ulid createdBy)
    => new()
    {
      NotificationId = Ulid.NewUlid(),
      UserId = Ulid.Parse(request.UserId),
      Title = request.Title,
      Message = request.Message,
      CreatedBy = createdBy,
    };

  public GetNotificationByIdResponse ToGetById()
    => new()
    {
      NotificationId = NotificationId.ToString(),
      Title = Title,
      Message = Message,
      IsUnread = IsUnread,
    };
}