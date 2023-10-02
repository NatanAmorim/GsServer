namespace gs_server.Dtos.Professores;

/// <summary>
/// Returns a professor object
/// </summary>
public class ResponseProfessorDto
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required string DataNascimento { get; set; }
  public required string Celular { get; set; }
  public required string Cep { get; set; }
  public required string Endereco { get; set; }
  public required string Numero { get; set; }
  public required string Cpf { get; set; }
}