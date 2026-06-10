using FinTech.Application.DTOs.Loan;

namespace FinTech.Application.Interfaces;

public interface ILoanService
{
    Task<SimulateLoanResponse> SimulateAsync(SimulateLoanRequest request);
    Task<LoanResponse> CreateAsync(CreateLoanRequest request);
    Task<IEnumerable<LoanResponse>> GetAllAsync(string? userId = null);
    Task<LoanResponse?> GetByIdAsync(Guid id);
    Task<List<PaymentScheduleResponse>> GetScheduleAsync(Guid id);
    Task<LoanResponse> ApproveAsync(Guid id);
    Task<LoanResponse> RejectAsync(Guid id);
}
