namespace GsServer.Models;

public class AttendanceModel
{
  public int AttendanceId { get; init; }
  public required int DisciplineId { get; init; }
  public required DateOnly Date { get; set; }
  public List<int> PresentStudentIds { get; set; } = [];
  public List<int> AbsentStudentIds { get; set; } = [];
  public string Observations { get; set; } = string.Empty;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
