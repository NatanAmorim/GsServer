using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GsServer.Protobufs;

namespace GsServer.Models;

[Index(nameof(Date), IsUnique = false)]
public class Attendance
{
  [Key]
  public Ulid AttendanceId { get; init; } = Ulid.NewUlid();
  [ForeignKey(nameof(DisciplineId))]
  public required Ulid DisciplineId { get; init; }
  public virtual Discipline Discipline { get; init; } = null!;
  [Required(ErrorMessage = "A data é obrigatória")]
  public required DateOnly Date { get; set; }
  [Required(ErrorMessage = "Os status dos participantes são obrigatórios")]
  public required ICollection<AttendanceAttendeeStatus> AttendeesStatuses { get; set; }
  [MaxLength(240, ErrorMessage = "As observações devem ter no máximo 240 caracteres")]
  [Required(ErrorMessage = "As observações são obrigatórias", AllowEmptyStrings = true)]
  public string Observations { get; set; } = string.Empty;
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; init; }

  public static Attendance FromProtoRequest(CreateAttendanceRequest request, Ulid createdBy)
    => new()
    {
      DisciplineId = Ulid.Parse(request.DisciplineId),
      Date = request.Date,
      AttendeesStatuses = request.AttendeesStatuses.Select(
          Installment => new AttendanceAttendeeStatus
          {
            AttendanceAttendeeStatusId = Ulid.Parse(Installment.AttendanceAttendeeStatusId),
            PersonId = Ulid.Parse(Installment.PersonId),
            IsPresent = Installment.IsPresent,
          }
        ).ToList(),
      Observations = request.Observations,
      CreatedBy = createdBy,
    };

  public GetAttendanceByIdResponse ToGetById()
  => new()
  {
    AttendanceId = AttendanceId.ToString(),
    Discipline = Discipline.ToGetById(),
    Date = Date,
    AttendeesStatuses = {
      AttendeesStatuses.Select(
        AttendeeStatus => new Protobufs.AttendanceAttendeeStatus
        {
          AttendanceAttendeeStatusId = AttendeeStatus.AttendanceAttendeeStatusId.ToString(),
          PersonId = AttendeeStatus.PersonId.ToString(),
          IsPresent = AttendeeStatus.IsPresent,
        }
      ).ToList(),
    },
    Observations = Observations,
  };

}
