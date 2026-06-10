namespace FinTech.Application.DTOs.Loan;

public class SimulateLoanResponse
{
    public decimal Amount { get; set; }
    public int Term { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyRate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal TotalInterest { get; set; }
    public List<PaymentScheduleResponse> Schedule { get; set; } = new();
}
