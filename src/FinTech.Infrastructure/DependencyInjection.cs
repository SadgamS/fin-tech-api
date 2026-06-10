using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FinTech.Infrastructure.Data;
using FinTech.Domain.Interfaces;
using FinTech.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

}
