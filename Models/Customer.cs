namespace gs_server.Models;

public class CustomerModel
{
  public int Id { get; init; }
  public UserModel? User { get; set; }
  public required PersonModel Person { get; set; }
  public required string PhysicalAddress { get; set; }
  public string? PicturePath { get; set; }
  public required string Pix { get; set; }
  public required List<PersonModel> Dependents { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}