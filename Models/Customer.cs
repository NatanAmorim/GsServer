namespace gs_server.Models;

public class Customer
{
  public required int Id { get; init; }
  public User? User { get; set; }
  public required Person Person { get; set; }
  public required Address Address { get; set; }
  public string? Picture { get; set; }
  public required string Pix { get; set; }
  public required List<Ward> Wards { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

// A legal guardian may or may not be related to the child by blood.
// A legal guardian might not be related to the child at all but may be
// appointed by the court to make decisions for the child when the biological
// parents cannot perform parental functions.
// “ward of the guardian.” conveys the idea that the child is under the
// care and protection of the legal guardian.
public class Ward
{
  public required int Id { get; init; }
  public required string Name { get; set; }
  public required string BirthDate { get; set; }
}