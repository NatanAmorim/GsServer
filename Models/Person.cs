namespace gs_server.Models;

public class PersonModel
{
  public int PersonId { get; init; }
  public required string Name { get; set; }
  public required string MobilePhoneNumber { get; set; }
  public required string BirthDate { get; set; }
  public required string Cpf { get; set; } // Cadastro de Pessoas Físicas (CPF)
  public required string Cin { get; set; } // Carteira de Identidade Nacional (CIN)
  public required List<PersonModel> Dependents { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}