namespace gs_server;

public class Endereco
{
  public required int Id { get; init; }
  public required string Logadouro { get; set; }
  public required string Numero { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}