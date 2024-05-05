using System.ComponentModel.DataAnnotations;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Instructor
{
  [Key]
  public Ulid InstructorId { get; init; } = Ulid.NewUlid();
  [Required(ErrorMessage = "A pessoa é obrigatória")]
  public required Person Person { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }

  public static Instructor FromProtoRequest(CreateInstructorRequest request, Ulid createdBy)
    => new()
    {
      Person = new Person
      {
        FullName = request.Person.Name,
        MobilePhoneNumber = request.Person.MobilePhoneNumber,
        BirthDate = request.Person.BirthDate,
        Cpf = request.Person.Cpf,
        CreatedBy = createdBy,
      },
      CreatedBy = createdBy,
    };

  public GetInstructorByIdResponse ToGetById()
    => new()
    {
      InstructorId = InstructorId.ToString(),
      Person = new()
      {
        Name = Person.FullName,
        MobilePhoneNumber = Person.MobilePhoneNumber,
        BirthDate = Person.BirthDate,
        Cpf = Person.Cpf,
      },
    };
}
