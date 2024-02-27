namespace gs_server.Models;

public class Person
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public required string MobilePhone { get; set; }
  public required string BirthDate { get; set; }
  public required string Cpf { get; set; }
  public required string Cin { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}