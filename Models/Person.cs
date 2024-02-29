namespace gs_server.Models;

public class Person
{
  public int Id { get; init; }
  public required string Name { get; set; }
  public required string MobilePhoneNumber { get; set; }
  public required string BirthDate { get; set; }
  public required string Cpf { get; set; } // Cadastro de Pessoas Físicas (CPF)
  public required string Cin { get; set; } // Carteira de Identidade Nacional (CIN)
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}