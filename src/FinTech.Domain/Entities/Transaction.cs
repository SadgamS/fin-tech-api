using FinTech.Domain.Common;
using FinTech.Domain.Enums;

namespace FinTech.Domain.Entities;

public class Transaction : BaseAuditableEntity
{
    public string IdempotencyKey { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public Guid? LoanId { get; set; }
    public string? Description { get; set; }

    public Loan? Loan { get; set; }
}
