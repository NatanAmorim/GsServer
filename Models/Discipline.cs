using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(StartTime), nameof(EndTime), nameof(IsActive), IsUnique = false)]
public class Discipline
{
  public int DisciplineId { get; init; }
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [MaxLength(16, ErrorMessage = "O nome deve ter no máximo 16 caracteres")]
  public required string Name { get; set; }
  [Column(TypeName = "decimal(19, 4)")]
  public required decimal TuitionPrice { get; set; }
  public required int InstructorId { get; set; }
  public virtual Instructor Instructor { get; set; } = null!;
  public required TimeOnly StartTime { get; set; }
  public required TimeOnly EndTime { get; set; }
  public required ICollection<DayOfWeek> ClassDays { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
