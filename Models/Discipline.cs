namespace gs_server.Models;

public class Discipline
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public required int TuitionPrice { get; set; }
  public required Teacher Teacher { get; set; }
  public required TimeOnly StartTime { get; set; }
  public required TimeOnly EndTime { get; set; }
  public required List<DayOfWeek> ClassDays { get; set; } // Dias de aula.
  public List<Customer> Students { get; set; } = [];
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public enum Weekday // Dia da semana
{
  Sunday = 0, // Domingo
  Monday = 1, // Segunda-feira
  Tuesday = 2, // Terça-feira
  Wednesday = 3, // Quarta-feira
  Thursday = 4, // Quinta-feira
  Friday = 5, // Sexta-feira
  Saturday = 6, // Sábado
}