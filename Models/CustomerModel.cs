namespace GsServer.Models;

public class CustomerModel
{
  public int CustomerId { get; init; }
  public UserModel? User { get; set; }
  public required PersonModel Person { get; set; }
  public required List<PersonModel> Dependents { get; set; }
  public required string BillingAddress { get; set; }
  // Image path on a Cloud Storage (Like: Imgur, S3, Azure blob).
  // All images will be scaled to 128px(w) x 128px(h).
  public string? PicturePath { get; set; }
  public required string AdditionalInformation { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }
}
