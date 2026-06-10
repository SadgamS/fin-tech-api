using FinTech.Application.DTOs.Transaction;
using FinTech.Domain.Enums;

namespace FinTech.Application.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(CreateTransactionRequest request);
    Task<IEnumerable<TransactionResponse>> GetAllAsync(
        TransactionType? type = null,
        TransactionStatus? status = null);
    Task<TransactionResponse?> GetByIdAsync(Guid id);
}
