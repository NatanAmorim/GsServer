using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(HasFinished), IsUnique = false)]
public class BackgroundJobStatus
{
  public int BackgroundJobStatusId { get; init; }
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [MaxLength(64, ErrorMessage = "O nome deve ter no máximo 64 caracteres")]
  public required string Name { get; set; }
  public bool HasFinished { get; set; } = false;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}