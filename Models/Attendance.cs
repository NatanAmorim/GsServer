using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(Date), IsUnique = false)]
public class Attendance
{
  [Key]
  public Ulid AttendanceId { get; init; } = Ulid.NewUlid();
  [ForeignKey(nameof(DisciplineId))]
  public required Ulid DisciplineId { get; init; }
  public virtual Discipline Discipline { get; init; } = null!;
  [Required(ErrorMessage = "A data é obrigatória")]
  public required DateOnly Date { get; set; }
  [Required(ErrorMessage = "Os status dos participantes são obrigatórios")]
  public required ICollection<AttendanceAttendeeStatus> AttendeesStatuses { get; set; }
  [MaxLength(240, ErrorMessage = "As observações devem ter no máximo 240 caracteres")]
  [Required(ErrorMessage = "As observações são obrigatórias", AllowEmptyStrings = true)]
  public string Observations { get; set; } = string.Empty;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public Ulid? CreatedBy { get; set; }
}
