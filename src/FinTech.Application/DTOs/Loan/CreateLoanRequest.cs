namespace FinTech.Application.DTOs.Loan;

public class CreateLoanRequest
{
    public string UserId { get; set; } = "user-123";
    public decimal Amount { get; set; }
    public int Term { get; set; }
    public decimal InterestRate { get; set; } = 0.24m;
    public string LoanType { get; set; } = "Fixed";
    public decimal MonthlyIncome { get; set; }
}
