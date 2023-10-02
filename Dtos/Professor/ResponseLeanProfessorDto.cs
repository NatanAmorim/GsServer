namespace gs_server.Dtos.Professores;

/// <summary>
/// Returns a lean professor object
/// </summary>
public class ResponseLeanProfessorDto
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
}