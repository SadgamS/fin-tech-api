using FinTech.Domain.Common;
using FinTech.Domain.Enums;

namespace FinTech.Domain.Entities;

public class Loan :  BaseAuditableEntity
{
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public int Term { get; set; }
    public decimal InterestRate { get; set; }
    public LoanType LoanType { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public decimal MonthlyPayment { get; set; }

    public ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new List<PaymentSchedule>();
    public ICollection<Trasaction> Transactions { get; set; } = new List<Trasaction>();

}
