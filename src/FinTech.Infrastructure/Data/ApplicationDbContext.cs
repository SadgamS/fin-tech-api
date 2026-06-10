using FinTech.Domain.Entities;
using FinTech.Infrastructure.Data.Configurations;
using FinTech.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<PaymentSchedule> PaymentSchedules => Set<PaymentSchedule>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new LoanConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentScheduleConfiguration());

        modelBuilder.ApplyConfiguration(new TransactionConfiguration());

        modelBuilder.ApplyConfiguration(new LoanSeed());
        modelBuilder.ApplyConfiguration(new TransactionSeed());
    }

}
