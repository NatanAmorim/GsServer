using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Instructor
{
  public int InstructorId { get; init; }
  public required Person Person { get; set; }
  public int? UserId { get; set; }
  public virtual User User { get; set; } = null!;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
