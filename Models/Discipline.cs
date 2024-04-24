using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Index(nameof(StartTime), nameof(EndTime), nameof(IsActive), IsUnique = false)]
public class Discipline
{
  [Key]
  public Ulid DisciplineId { get; init; } = Ulid.NewUlid();
  [MinLength(4, ErrorMessage = "O nome deve ter no mínimo 4 caracteres")]
  [MaxLength(16, ErrorMessage = "O nome deve ter no máximo 16 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o nome", AllowEmptyStrings = false)]
  public required string Name { get; set; }
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O preço da mensalidade não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O preço da mensalidade é obrigatório")]
  public required decimal TuitionPrice { get; set; }
  [ForeignKey(nameof(InstructorId))]
  public required Ulid InstructorId { get; set; }
  public virtual Instructor Instructor { get; set; } = null!;
  [Required(ErrorMessage = "O horário de início é obrigatório")]
  public required TimeOnly StartTime { get; set; }
  [Required(ErrorMessage = "O horário de início é obrigatório")]
  public required TimeOnly EndTime { get; set; }
  [Required(ErrorMessage = "Os dias de aula são obrigatórios")]
  public required ICollection<DayOfWeek> ClassDays { get; set; }
  public bool IsActive { get; set; } = true;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
}
