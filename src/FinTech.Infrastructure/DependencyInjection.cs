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
        var connectionString = GetConnectionString(configuration);

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        // Railway provee DATABASE_URL como variable de entorno
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (!string.IsNullOrEmpty(databaseUrl))
            return ConvertDatabaseUrlToConnectionString(databaseUrl);

        return configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    private static string ConvertDatabaseUrlToConnectionString(string databaseUrl)
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');

        return $"Host={uri.Host};" +
               $"Port={uri.Port};" +
               $"Database={uri.AbsolutePath.TrimStart('/')};" +
               $"Username={userInfo[0]};" +
               $"Password={userInfo[1]};" +
               $"SSL Mode=Require;" +
               $"Trust Server Certificate=true;";
    }

}
