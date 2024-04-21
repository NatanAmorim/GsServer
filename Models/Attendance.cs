using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(Date), IsUnique = false)]
public class Attendance
{
  public int AttendanceId { get; init; }
  public required int DisciplineId { get; init; }
  public virtual Discipline Discipline { get; init; } = null!;
  public required DateOnly Date { get; set; }
  public required ICollection<AttendanceAttendeeStatus> AttendeesStatuses { get; set; }
  [MaxLength(240, ErrorMessage = "As Observações devem ter no máximo 240 caracteres")]
  public string Observations { get; set; } = string.Empty;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
