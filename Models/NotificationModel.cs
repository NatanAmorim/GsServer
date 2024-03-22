namespace GsServer.Models;

// Sends reminders for upcoming events, invoice expirations, etc.
public class NotificationModel
{
  public int NotificationId { get; init; }
  public int UserId { get; init; }
  public required string Title { get; set; }
  public required string Message { get; set; }
  public bool IsUnread { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}