using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Instructor
{
  [Key]
  public Ulid InstructorId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "A pessoa é obrigatória")]
  public required Person Person { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public Ulid? CreatedBy { get; set; }
}
