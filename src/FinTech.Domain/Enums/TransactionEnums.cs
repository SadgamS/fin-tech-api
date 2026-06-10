namespace FinTech.Domain.Enums;

public enum TransactionType
{
    Disbursement,
    Payment,
    Transfer
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}