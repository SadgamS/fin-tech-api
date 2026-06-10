using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Data.Seeds;

public class LoanSeed : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        var loan1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var loan2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

        builder.HasData(
            new Loan
            {
                Id = loan1Id,
                UserId = "user-123",
                Amount = 5000,
                Term = 12,
                InterestRate = 0.24m,
                LoanType = LoanType.Fixed,
                Status = LoanStatus.Active,
                MonthlyPayment = 472.07m,
                MonthlyIncome = 3000,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Loan
            {
                Id = loan2Id,
                UserId = "user-456",
                Amount = 2000,
                Term = 6,
                InterestRate = 0.24m,
                LoanType = LoanType.Fixed,
                Status = LoanStatus.Pending,
                MonthlyPayment = 356.04m,
                MonthlyIncome = 2000,
                CreatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
