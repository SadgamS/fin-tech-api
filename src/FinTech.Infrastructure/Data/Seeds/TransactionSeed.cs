using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Data.Seeds;

public class TransactionSeed : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        var loan1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        builder.HasData(
            new Transaction
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                IdempotencyKey = "seed-disbursement-001",
                Type = TransactionType.Disbursement,
                Amount = 5000,
                Status = TransactionStatus.Completed,
                LoanId = loan1Id,
                Description = "Desembolso préstamo seed",
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
