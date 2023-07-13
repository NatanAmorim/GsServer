using gs_server.Models.Clientes;
using gs_server.Models.Professores;

namespace gs_server.Models.Aulas;

public class Aula
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required string Preco { get; set; }
  public required Professor Professor { get; set; }
  public required TimeOnly HoraInicio { get; set; }
  public required TimeOnly HoraFim { get; set; }
  public required List<Cliente> Alunos { get; set; }
  public required bool IsAtiva { get; set; } = false;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}