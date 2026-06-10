namespace FinTech.Application;

using FinTech.Application.Interfaces;
using FinTech.Application.Services;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ILoanService, LoanService>();
        services.AddScoped<ITransactionService, TransactionService>();
        return services;
    }
}
