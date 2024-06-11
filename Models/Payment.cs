using System.ComponentModel.DataAnnotations;
using GsServer.Protobufs;

namespace GsServer.Models;

public class Payment
{
  [Key]
  public required Ulid PaymentId { get; init; }
  /// <summary>
  /// Comments is for things like installment price changed because of returned item.
  /// </summary>
  [MaxLength(240, ErrorMessage = "O comentário deve ter no máximo 240 caracteres")]
  [Required(AllowEmptyStrings = true)]
  public required string Observations { get; set; }
  [Required(ErrorMessage = "As parcelas são obrigatórias")]
  public required ICollection<PaymentInstallment> Installments { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  [Required]
  public required Ulid CreatedBy { get; set; }
  public decimal TotalAmountOwed()
  {
    throw new NotImplementedException();
  }
  public decimal TotalAmountPaid()
  {
    throw new NotImplementedException();
  }

  public static Payment FromProtoRequest(CreatePaymentRequest request, Ulid createdBy)
    => new()
    {
      PaymentId = Ulid.NewUlid(),
      Observations = request.Observations,
      Installments = request.Installments.Select(
        Installment => new PaymentInstallment
        {
          PaymentInstallmentId = Ulid.Parse(Installment.PaymentInstallmentId),
          InstallmentNumber = Installment.InstallmentNumber,
          InstallmentAmount = Installment.InstallmentAmount,
          PaymentMethod = Installment.PaymentMethod,
          DueDate = new(
            Installment.DueDate.Year,
            Installment.DueDate.Month,
            Installment.DueDate.Day
          ),
        }
      ).ToList(),
      CreatedBy = createdBy,
    };

  public GetPaymentByIdResponse ToGetById()
    => new()
    {
      PaymentId = PaymentId.ToString(),
      Observations = Observations,
      Installments = {
        Installments.Select(
          Installment => new Protobufs.PaymentInstallment
          {
            PaymentInstallmentId = Installment.PaymentInstallmentId.ToString(),
            InstallmentNumber = Installment.InstallmentNumber,
            InstallmentAmount = Installment.InstallmentAmount,
            PaymentMethod = Installment.PaymentMethod,
            DueDate = Installment.DueDate,
          }
        ).ToList(),
      },
    };
}

// TODO Implement promotional offers (discounts, trials, upgrades).
// TODO add Subscription History
