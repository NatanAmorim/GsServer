namespace gs_server.Models;

public class Cliente
{
  public required int Id { get; init; }
  public required Pessoa Pessoa { get; set; }
  public Usuario? Usuario { get; set; }
  public string? Foto { get; set; }
  public required string NomePix { get; set; }
  public required List<ClienteDependente> Dependentes { get; set; }
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