namespace gs_server.Models;

public class InstructorModel
{
  public int InstructorId { get; init; }
  public required PersonModel Person { get; set; }
  public required UserModel User { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
