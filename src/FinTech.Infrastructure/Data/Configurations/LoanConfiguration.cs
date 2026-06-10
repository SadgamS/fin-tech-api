using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Data.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Amount).HasPrecision(18, 2);
        builder.Property(l => l.InterestRate).HasPrecision(8, 6);
        builder.Property(l => l.MonthlyPayment).HasPrecision(18, 2);
        builder.Property(l => l.MonthlyIncome).HasPrecision(18, 2);
        builder.Property(l => l.LoanType).HasConversion<string>();
        builder.Property(l => l.Status).HasConversion<string>();
        builder.Property(l => l.UserId).HasMaxLength(100);
    }
}
