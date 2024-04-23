using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

public class AttendanceAttendeeStatus
{
  public int AttendanceAttendeeStatusId { get; init; }
  [ForeignKey(nameof(AttendanceId))]
  public int AttendanceId { get; init; }
  [ForeignKey(nameof(PersonId))]
  public required int PersonId { get; init; }
  public virtual Person Person { get; init; } = null!;
  [Required(ErrorMessage = "O status presente/ausente é obrigatório")]
  public required bool IsPresent { get; init; }
  public bool IsAbsent() => !IsPresent;
}
