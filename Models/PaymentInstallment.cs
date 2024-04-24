using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GsServer.Models;

/*
The installment buying definition refers to the process of purchasing an asset
over time. When you agree to an installment purchase plan, you acquire the asset
on the same day and then pay for it in a series of periodic installment.
Installment buying is similar to a credit purchase, each spreading the cost of a
purchase out over time.

However, there are more restrictions involved with an installment plan. You
agree to make installment payments until the purchase is paid off, usually with
a preset amount and date of the month. Purchases made with a credit card are
more flexible, provided you meet your monthly minimum.

There are three main types of installment payments:
- Installment sales - an agreement between the merchant, and the customer allowing them to split the cost of their purchase into three or four smaller installments paid at regular intervals.
- Installment loans - your customer receives a loan from a third party in order to fund their purchase and then pays the loan back in installments, plus interest. This is usually for higher value items and the repayment period can be over months or even years.
- Pay later - the entire purchase cost is delayed for a period (usually between 14 to 30 days) and then repaid in full.
- Pay on delivery - your customer pays for goods after they have been delivered. An example of this is the service
*/
[Owned]
public class PaymentInstallment
{
  [Key]
  public Ulid PaymentInstallmentId { get; init; } = Ulid.NewUlid();
  [Range(1, 18, ErrorMessage = "O número da parcela não pode ser mais do que 18")]
  public int InstallmentNumber { get; set; } // Sequential number, (e.g, "${installment.InstallmentNumber} of {payment.Installments.Count}" = “2 of 6”)
  [Column(TypeName = "decimal(8, 4)")]
  [Range(1, 999_999.99, ErrorMessage = "O valor da parcela não deve ser menos que R$ 1,00 ou exceder R$ 999999,99")]
  [Required(ErrorMessage = "O valor da parcela é obrigatório")]
  public decimal InstallmentAmount { get; set; }
  [MinLength(2, ErrorMessage = "O método de pagamento deve ter no mínimo 2 caracteres")]
  [MaxLength(16, ErrorMessage = "O método de pagamento deve ter no máximo 16 caracteres")]
  [Required(ErrorMessage = "Obrigatório preencher o método de pagamento", AllowEmptyStrings = false)]
  public required string PaymentMethod { get; set; } // (e.g., "money", "credit card", "debit card", ...).
  public DateOnly DueDate { get; set; } // Optional property for due date
}