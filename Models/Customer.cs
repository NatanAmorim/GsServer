namespace gs_server.Models;

public class Customer
{
  public required int Id { get; init; }
  public User? User { get; set; }
  public required Person Person { get; set; }
  public required Address Address { get; set; }
  public byte[]? Picture { get; set; }
  public required string Pix { get; set; }
  public required List<Person> Dependents { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}