using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

[Owned]
public class Dependent
{
  [Key]
  public required Ulid DependentId { get; init; }
  [MinLength(5, ErrorMessage = "O nome completo deve ter no mínimo 5 caracteres")]
  [MaxLength(55, ErrorMessage = "O nome completo deve ter no máximo 55 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string FullName { get; set; }
  [MinLength(15, ErrorMessage = "O número de celular deve ter no mínimo 15 caracteres")]
  [MaxLength(16, ErrorMessage = "O número de celular deve ter no máximo 16 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string BirthDate { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Dependent FromProtoRequest(Protobufs.Dependent request, Ulid createdBy)
    => new()
    {
      DependentId = Ulid.NewUlid(),
      FullName = request.Name,
      BirthDate = request.BirthDate,
      CreatedBy = createdBy,
    };

  public Protobufs.Dependent ToDependentById()
    => new()
    {
      DependentId = DependentId.ToString(),
      Name = FullName,
      BirthDate = BirthDate,
    };
}