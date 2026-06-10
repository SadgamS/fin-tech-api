using FinTech.Domain.Entities;
using FinTech.Domain.Interfaces;

namespace FinTech.Domain.Strategies;

public class FixedLoanStrategy : ILoanCalculationStrategy
{
    public decimal CalculateMonthlyPayment(decimal amount, decimal tem, int termMonths)
    {
        decimal factor = (decimal)Math.Pow((double)(1 + tem), termMonths);
        return Math.Round(amount * tem * factor / (factor - 1), 2);
    }

    public List<PaymentSchedule> GenerateSchedule(Guid loanId, decimal amount, decimal tem, int termMonths, DateTime startDate)
    {
        decimal monthlyPayment = CalculateMonthlyPayment(amount, tem, termMonths);
        var schedule = new List<PaymentSchedule>();
        decimal balance = amount;

        for (int i = 1; i <= termMonths; i++)
        {
            decimal interest = Math.Round(balance * tem, 2);
            decimal principal = (i == termMonths) ? balance : Math.Round(monthlyPayment - interest, 2);
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
