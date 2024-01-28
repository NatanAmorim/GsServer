namespace gs_server.Models;

public class Pessoa
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required string Celular { get; set; }
  public required string DataNascimento { get; set; }
  public required string Cpf { get; set; }
  public required string Cep { get; set; }
  public required Endereco Endereco { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}