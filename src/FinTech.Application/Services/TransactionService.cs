using FinTech.Application.DTOs.Transaction;
using FinTech.Application.Interfaces;
using FinTech.Domain.Enums;
using FinTech.Domain.Interfaces;
using FinTech.Domain.Entities;

namespace FinTech.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepo;

    public TransactionService(ITransactionRepository transactionRepo)
    {
        _transactionRepo = transactionRepo;
    }

    public async Task<(TransactionResponse transaction, bool isNew)> CreateAsync(CreateTransactionRequest request)
    {
        // Idempotencia: si ya existe con esta key, retornar la original
        var existing = await _transactionRepo.GetByIdempotencyKeyAsync(request.IdempotencyKey);
        if (existing != null)
            return (MapTransaction(existing), false);

        if (!Enum.TryParse<TransactionType>(request.Type, true, out var type))
            throw new ArgumentException("Tipo de transacción inválido");

        var transaction = new Transaction
        {
            IdempotencyKey = request.IdempotencyKey,
            Type = type,
            Amount = request.Amount,
            Status = TransactionStatus.Completed,
            LoanId = request.LoanId,
            Description = request.Description
        };

        await _transactionRepo.CreateAsync(transaction);
        return (MapTransaction(transaction), true);
    }

    public async Task<IEnumerable<TransactionResponse>> GetAllAsync(
        TransactionType? type = null,
        TransactionStatus? status = null)
    {
        var transactions = await _transactionRepo.GetAllAsync(type, status);
        return transactions.Select(MapTransaction);
    }

    public async Task<TransactionResponse?> GetByIdAsync(Guid id)
    {
        var transaction = await _transactionRepo.GetByIdAsync(id);
        return transaction == null ? null : MapTransaction(transaction);
    }


    private static TransactionResponse MapTransaction(Transaction t) => new()
    {
        Id = t.Id,
        IdempotencyKey = t.IdempotencyKey,
        Type = t.Type.ToString(),
        Amount = t.Amount,
        Status = t.Status.ToString(),
        LoanId = t.LoanId,
        Description = t.Description,
        CreatedAt = t.CreatedAt
    };
}
