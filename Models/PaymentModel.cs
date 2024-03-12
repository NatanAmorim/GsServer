namespace gs_server.Models;

public class PaymentModel
{
  public int PaymentId { get; init; }
  public required string Observations { get; set; } // For things like installment price changed because of returned item.
  public required List<PaymentInstallmentModel> Installments { get; set; }
  public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
  public required int CreatedBy { get; init; }

  public decimal TotalAmountOwed()
  {
    throw new NotImplementedException();
  }

  public decimal TotalAmountPaid()
  {
    throw new NotImplementedException();
  }
}

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
public class PaymentInstallmentModel
{
  public int PaymentInstallmentId { get; init; }
  public int PaymentId { get; set; }
  public int InstallmentNumber { get; set; } // Sequential number, (e.g, "${installment.InstallmentNumber} of {payment.Installments.Count}" = “2 of 6”)
  public decimal InstallmentAmount { get; set; }
  public required string PaymentMethod { get; set; } // (e.g., "money", "credit card", "debit card", ...).
  public DateOnly? DueDate { get; set; } // Optional property for due date
}

// TODO Implement promotional offers (discounts, trials, upgrades).
// TODO add Subscription History
