using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FinTech.Domain.Interfaces;
using FinTech.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(
        TransactionType? type = null,
        TransactionStatus? status = null)
    {
        var query = _context.Transactions.AsQueryable();
        if (type.HasValue) query = query.Where(t => t.Type == type);
        if (status.HasValue) query = query.Where(t => t.Status == status);
        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id) =>
        await _context.Transactions.FindAsync(id);

    public async Task<Transaction?> GetByIdempotencyKeyAsync(string key) =>
        await _context.Transactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == key);

    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> UpdateAsync(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }


}
