using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Instructor
{
  public int InstructorId { get; init; }
  [Required(ErrorMessage = "A pessoa é obrigatória")]
  public required Person Person { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
