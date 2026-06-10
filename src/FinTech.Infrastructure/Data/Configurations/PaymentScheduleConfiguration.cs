using FinTech.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinTech.Infrastructure.Data.Configurations;

public class PaymentScheduleConfiguration : IEntityTypeConfiguration<PaymentSchedule>
{
    public void Configure(EntityTypeBuilder<PaymentSchedule> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.TotalPayment).HasPrecision(18, 2);
        builder.Property(ps => ps.Principal).HasPrecision(18, 2);
        builder.Property(ps => ps.Interest).HasPrecision(18, 2);
        builder.Property(ps => ps.RemainingBalance).HasPrecision(18, 2);
        builder.Property(ps => ps.Status).HasConversion<string>();
        builder.HasOne(ps => ps.Loan)
            .WithMany(l => l.PaymentSchedules)
            .HasForeignKey(ps => ps.LoanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
