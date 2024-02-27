namespace gs_server;

public class Address
{
  public required int Id { get; init; }
  public required string StreetAddress { get; set; } // Logadouro.
  public required string PostalCode { get; set; }
  public required string Country { get; set; }
  public required string State { get; set; }
  public required string City { get; set; }
  public required string AdditionalDetails { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}