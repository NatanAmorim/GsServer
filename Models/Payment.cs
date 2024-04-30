using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Payment
{
  [Key]
  public Ulid PaymentId { get; init; } = Ulid.NewUlid();
  /// <summary>
  /// Comments is for things like installment price changed because of returned item.
  /// </summary>
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(ErrorMessage = "O comentário é obrigatório", AllowEmptyStrings = true)]
  public required string Observations { get; set; }
  [Required(ErrorMessage = "As parcelas são obrigatórias")]
  public required ICollection<PaymentInstallment> Installments { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public Ulid? CreatedBy { get; set; }
  public decimal TotalAmountOwed()
  {
    throw new NotImplementedException();
  }
  public decimal TotalAmountPaid()
  {
    throw new NotImplementedException();
  }
}

// TODO Implement promotional offers (discounts, trials, upgrades).
// TODO add Subscription History
