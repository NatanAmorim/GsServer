using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(HasFinished), IsUnique = false)]
public class BackgroundJobStatus
{
  public int BackgroundJobStatusId { get; init; }
  [MinLength(4)]
  [MaxLength(64)]
  [Required(AllowEmptyStrings = false)]
  public required string Name { get; set; }
  public bool HasFinished { get; set; } = false;
  public DateTime? FinishedAt { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}