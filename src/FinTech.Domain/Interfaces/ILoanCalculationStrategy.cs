using FinTech.Domain.Entities;

namespace FinTech.Domain.Interfaces;

public interface ILoanCalculationStrategy
{
    decimal CalculateMonthlyPayment(decimal amount, decimal tem, int termMonths);
    List<PaymentSchedule> GenerateSchedule(
        Guid loanId,
        decimal amount,
        decimal tem,
        int termMonths,
        DateTime startDate);
}
