using FinTech.Domain.Common;
using FinTech.Domain.Constants;
using FinTech.Domain.Enums;
using FinTech.Domain.Strategies;

namespace FinTech.Domain.Entities;

public class Loan : BaseAuditableEntity
{
    public string UserId { get; set; }
    public decimal Amount { get; set; }
    public int Term { get; set; }
    public decimal InterestRate { get; set; }
    public LoanType LoanType { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public decimal MonthlyPayment { get; set; }
    public decimal MonthlyIncome { get; set; }

    public ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new List<PaymentSchedule>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


    public Loan(string userId, decimal amount, int term, decimal interestRate, LoanType loanType, decimal monthlyIncome)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("El id del usuario es requerido.", nameof(userId));

        if (amount < LoanConstants.MinimumAmount || amount > LoanConstants.MaximumAmount)
            throw new ArgumentOutOfRangeException(nameof(amount), $"El monto del préstamo debe estar entre {LoanConstants.MinimumAmount} y {LoanConstants.MaximumAmount}.");

        if (term < LoanConstants.MinimumTermInMonths || term > LoanConstants.MaximumTermInMonths)
            throw new ArgumentOutOfRangeException(nameof(term), $"El plazo del préstamo debe estar entre {LoanConstants.MinimumTermInMonths} y {LoanConstants.MaximumTermInMonths} meses.");

        if (monthlyIncome <= 0)
            throw new ArgumentOutOfRangeException(nameof(monthlyIncome), "El ingreso mensual debe ser mayor que cero.");

        if (!Enum.IsDefined(typeof(LoanType), loanType))
            throw new ArgumentException("Tipo de préstamo no válido.", nameof(loanType));

        UserId = userId;
        Amount = amount;
        Term = term;
        InterestRate = interestRate;
        LoanType = loanType;
        MonthlyIncome = monthlyIncome;
        MonthlyPayment = CalculateMonthlyPayment(loanType, amount, interestRate, term);
    }

    public void ValidateRules(int activeLoansCount, decimal totalExistingMonthlyPayments)
    {
        if (activeLoansCount >= LoanConstants.MaxActiveLoans)
            throw new InvalidOperationException($"El usuario no puede tener más de {LoanConstants.MaxActiveLoans} préstamos activos.");

        decimal totalPayments = totalExistingMonthlyPayments + MonthlyPayment;

        if (totalPayments > MonthlyIncome * LoanConstants.MaxPaymentIncomeRatio)
            throw new InvalidOperationException($"La suma de couotas existentes y el nuevo préstamo no puede exceder el {LoanConstants.MaxPaymentIncomeRatio * 100}% del ingreso mensual del usuario.");
    }


    public void ApplyAutoScoring(int activeLoansCount)
    {
        Status = (Amount < 10000m && activeLoansCount < 2) ? LoanStatus.Approved : LoanStatus.Pending;
    }

    private static decimal CalculateMonthlyPayment(LoanType loanType, decimal amount, decimal interestRate, int term)
    {
        var strategy = LoanStrategyFactory.GetStrategy(loanType);
        return strategy.CalculateMonthlyPayment(amount, interestRate, term);
    }

    public void GenerateSchedule(DateTime startDate)
    {
        var strategy = LoanStrategyFactory.GetStrategy(LoanType);
        PaymentSchedules = strategy.GenerateSchedule(Id, Amount, CalculateMonthlyRate(), Term, startDate);
    }

    private decimal CalculateMonthlyRate()
    {
        return (decimal)(Math.Pow((double)(1 + InterestRate), 1.0 / 12) - 1);
    }

}
