using FinTech.Domain.Entities;
using FinTech.Domain.Interfaces;

namespace FinTech.Domain.Strategies;

public class DecreasingLoanStrategy : ILoanCalculationStrategy
{
    public decimal CalculateMonthlyPayment(decimal amount, decimal tem, int termMonths)
    {
        decimal amortization = Math.Round(amount / termMonths, 2);
        decimal firstInterest = Math.Round(amount * tem, 2);
        return amortization + firstInterest;
    }

    public List<PaymentSchedule> GenerateSchedule(Guid loanId, decimal amount, decimal tem, int termMonths, DateTime startDate)
    {
        var amortization = Math.Round(amount / termMonths, 2);
        var schedule = new List<PaymentSchedule>();
        var balance = amount;

        for (int i = 1; i <= termMonths; i++)
        {
            var interest = Math.Round(balance * tem, 2);
            var principal = (i == termMonths) ? balance : amortization;
            balance = Math.Round(balance - principal, 2);

            schedule.Add(new PaymentSchedule
            {
                Id = Guid.NewGuid(),
                LoanId = loanId,
                PaymentNumber = i,
                DueDate = GetDueDate(startDate, i),
                TotalPayment = Math.Round(principal + interest, 2),
                Principal = principal,
                Interest = interest,
                RemainingBalance = balance < 0 ? 0 : balance,
                Status = Enums.PaymentScheduleStatus.Pending
            });
        }

        return schedule;
    }

    private static DateTime GetDueDate(DateTime startDate, int monthOffset)
    {
        var dueDate = startDate.AddMonths(monthOffset);
        var day = Math.Min(startDate.Day, DateTime.DaysInMonth(dueDate.Year, dueDate.Month));

        return new DateTime(dueDate.Year, dueDate.Month, day, 0, 0, 0, DateTimeKind.Utc);
    }
}
