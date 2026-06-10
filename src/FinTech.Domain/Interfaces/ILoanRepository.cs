using FinTech.Domain.Entities;

namespace FinTech.Domain.Interfaces;

public interface ILoanRepository
{
    Task<IEnumerable<Loan>> GetAllAsync(string? userId = null);
    Task<Loan?> GetByIdAsync(Guid id);
    Task<Loan?> GetByIdWithScheduleAsync(Guid id);
    Task<int> CountActiveByUserAsync(string userId);
    Task<decimal> SumMonthlyPaymentsByUserAsync(string userId);
    Task<Loan> CreateAsync(Loan loan);
    Task<Loan> UpdateAsync(Loan loan);
}
