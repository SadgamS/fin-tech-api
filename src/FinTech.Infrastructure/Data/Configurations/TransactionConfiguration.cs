using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Amount).HasPrecision(18, 2);
        builder.Property(t => t.Type).HasConversion<string>();
        builder.Property(t => t.Status).HasConversion<string>();
        builder.Property(t => t.IdempotencyKey).HasMaxLength(255);
        builder.HasIndex(t => t.IdempotencyKey).IsUnique();
        builder.HasOne(t => t.Loan)
            .WithMany(l => l.Transactions)
            .HasForeignKey(t => t.LoanId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
