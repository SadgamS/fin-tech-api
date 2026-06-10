using FinTech.Domain.Entities;
using FinTech.Domain.Enums;

namespace FinTech.Infrastructure.Data.Seeds;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context)
    {
        if (context.Loans.Any()) return;

        var loan = new Loan(
            userId: "user-001",
            amount: 5000m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        var loan2 = new Loan(
            userId: "user-002",
            amount: 10000m,
            term: 24,
            interestRate: 0.18m,
            loanType: LoanType.Fixed,
            monthlyIncome: 8000m
        );

        loan.ApplyAutoScoring(0);
        loan.GenerateSchedule(DateTime.UtcNow);

        loan2.ApplyAutoScoring(0);
        loan2.GenerateSchedule(DateTime.UtcNow);

        if (loan.Status == LoanStatus.Approved)
            loan.Status = LoanStatus.Active;

        context.Loans.Add(loan);
        context.Loans.Add(loan2);
        await context.SaveChangesAsync();
    }
}
