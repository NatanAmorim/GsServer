namespace gs_server.Models;

public class Aula
{
  public required int Id { get; init; }
  public required string Nome { get; set; }
  public required int Preco { get; set; }
  public required Professor Professor { get; set; }
  public required TimeOnly HoraInicio { get; set; }
  public required TimeOnly HoraFim { get; set; }
  public required List<Dias> DiasDaSemana { get; set; }
  public List<Cliente> Alunos { get; set; } = [];
  public bool IsAtiva { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required string CreatedBy { get; init; }
}

public enum Dias
{
  dom = 0, // Domingo
  seg = 1, // Segunda-feira
  ter = 2, // Terça-feira
  qua = 3, // Quarta-feira
  qui = 4, // Quinta-feira
  sex = 5, // Sexta-feira
  sab = 6, // Sábado
}