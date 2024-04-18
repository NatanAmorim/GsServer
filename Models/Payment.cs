using System.ComponentModel.DataAnnotations;

namespace GsServer.Models;

public class Payment
{
  public int PaymentId { get; init; }
  /// <summary>
  /// Comments is for things like installment price changed because of returned item.
  /// </summary>
  [Length(4, 240, ErrorMessage = "O comentário deve ter entre 4 e 240 caracteres")]
  public required string Comments { get; set; }
  public required ICollection<PaymentInstallment> Installments { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required(ErrorMessage = "CreatedBy é obrigatório")]
  public int? CreatedBy { get; set; }
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
