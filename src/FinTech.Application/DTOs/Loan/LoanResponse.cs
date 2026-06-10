namespace FinTech.Application.DTOs.Loan;

public class LoanResponse
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Term { get; set; }
    public decimal InterestRate { get; set; }
    public string LoanType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal MonthlyPayment { get; set; }
    public decimal MonthlyIncome { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<PaymentScheduleResponse> PaymentSchedules { get; set; } = new();
}
