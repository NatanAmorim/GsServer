using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

[Index(nameof(HasFinished), IsUnique = false)]
public class BackgroundJobStatus
{
  public int BackgroundJobStatusId { get; init; }
  [Length(4, 64, ErrorMessage = "O nome deve ter entre 4 e 64 caracteres")]
  public required string Name { get; set; }
  public bool HasFinished { get; set; } = false;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}