using gs_server.Models.Aulas;

namespace gs_server.Models.Clientes;

public class Cliente
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public string? Foto { get; set; }
  public required string Celular { get; set; }
  public required string DataNascimento { get; set; }
  public required string Cpf { get; set; }
  public required string Cep { get; set; }
  public required string Endereco { get; set; }
  public required string Numero { get; set; }
  public required string NomePix { get; set; }
  public required List<ClienteDependente> dependentes { get; set; }
  public required List<Aula> AulasInscritas { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public class ClienteDependente
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required string DataNascimento { get; set; }
}