namespace GsServer.Models;

public class AttendanceAttendeeStatus
{
  public int AttendanceAttendeeStatusId { get; init; }
  public int AttendanceId { get; init; }
  public required int PersonId { get; init; }
  public virtual Person Person { get; init; } = null!;
  public required bool IsPresent { get; init; }
  public bool IsAbsent() => !IsPresent;
}
