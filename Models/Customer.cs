using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Customer
{
  [Key]
  public required Ulid CustomerId { get; init; }
  [ForeignKey(nameof(UserId))]
  public Ulid? UserId { get; set; }
  public virtual User User { get; set; } = null!;
  [Required(ErrorMessage = "A Pessoa é obrigatória")]
  public required Person Person { get; set; }
  [Required(ErrorMessage = "Os dependentes são obrigatórios")]
  public required ICollection<Dependent> Dependents { get; set; }
  [MinLength(4, ErrorMessage = "O Endereço deve ter no mínimo 4 caracteres")]
  [MaxLength(120, ErrorMessage = "O Endereço deve ter no máximo 120 caracteres")]
  [Required(ErrorMessage = "Campo de preenchimento obrigatório", AllowEmptyStrings = false)]
  public required string BillingAddress { get; set; }
  [MaxLength(240, ErrorMessage = "As Informações adicionais devem ter no máximo 240 caracteres")]
  [Required(AllowEmptyStrings = true)]
  public required string AdditionalInformation { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Customer FromProtoRequest(CreateCustomerRequest request, Ulid createdBy)
    => new()
    {
      CustomerId = Ulid.NewUlid(),
      Person = Person.FromProtoRequest(request.Person, createdBy),
      Dependents =
        request.Dependents.Select(
          Dependent => new Models.Dependent
          {
            DependentId = Ulid.NewUlid(),
            FullName = Dependent.Name,
            BirthDate = Dependent.BirthDate,
            CreatedBy = createdBy,
          }
        ).ToList(),
      BillingAddress = request.BillingAddress,
      AdditionalInformation = request.AdditionalInformation,
      CreatedBy = createdBy,
    };

  public GetCustomerByIdResponse ToGetById()
    => new()
    {
      CustomerId = CustomerId.ToString(),
      Person = Person.ToPersonById(),
      Dependents = {
        Dependents.Select(
          Dependent => new Protobufs.Dependent
          {
            DependentId = Dependent.DependentId.ToString(),
            Name = Dependent.FullName,
            BirthDate = Dependent.BirthDate,
          }
        ).ToList(),
      },
      BillingAddress = BillingAddress,
      AdditionalInformation = AdditionalInformation,
    };
}
