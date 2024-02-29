namespace gs_server.Models;

public class Attendance
{
  public int Id { get; init; }
  public required int DisciplineId { get; init; }
  public required DateOnly Date { get; set; }
  public List<int> PresentStudents { get; set; } = [];
  public List<int> AbsentStudents { get; set; } = [];
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}