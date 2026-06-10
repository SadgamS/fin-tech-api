using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FinTech.Domain.Interfaces;
using FinTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly ApplicationDbContext _context;

    public LoanRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Loan>> GetAllAsync(string? userId = null)
    {
        var query = _context.Loans.AsQueryable();
        if (!string.IsNullOrEmpty(userId))
            query = query.Where(l => l.UserId == userId);
        return await query.OrderByDescending(l => l.CreatedAt).ToListAsync();
    }

    public async Task<Loan?> GetByIdAsync(Guid id) =>
        await _context.Loans.FindAsync(id);

    public async Task<Loan?> GetByIdWithScheduleAsync(Guid id) =>
        await _context.Loans
            .Include(l => l.PaymentSchedules.OrderBy(p => p.PaymentNumber))
            .FirstOrDefaultAsync(l => l.Id == id);

    public async Task<int> CountActiveByUserAsync(string userId) =>
        await _context.Loans.CountAsync(l =>
            l.UserId == userId &&
            (l.Status == LoanStatus.Active || l.Status == LoanStatus.Approved));

    public async Task<decimal> SumMonthlyPaymentsByUserAsync(string userId) =>
        await _context.Loans
            .Where(l => l.UserId == userId &&
                       (l.Status == LoanStatus.Active || l.Status == LoanStatus.Approved))
            .SumAsync(l => l.MonthlyPayment);

    public async Task<Loan> CreateAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

    public async Task<Loan> UpdateAsync(Loan loan)
    {
        loan.UpdatedAt = DateTime.UtcNow;
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

}
