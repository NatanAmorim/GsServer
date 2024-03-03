namespace gs_server.Models;

public class DisciplineModel
{
  public int Id { get; init; }
  public required string Name { get; set; }
  public required float TuitionPrice { get; set; }
  public required TeacherModel Teacher { get; set; }
  public required TimeOnly StartTime { get; set; }
  public required TimeOnly EndTime { get; set; }
  public required List<DayOfWeek> ClassDays { get; set; } // Dias de aula.
  public List<CustomerModel> Students { get; set; } = [];
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}

public enum DayOfWeek
{
  Sunday = 0,
  Monday = 1,
  Tuesday = 2,
  Wednesday = 3,
  Thursday = 4,
  Friday = 5,
  Saturday = 6,
}