using FinTech.Domain.Enums;
using FinTech.Domain.Interfaces;

namespace FinTech.Domain.Strategies;

public static class LoanStrategyFactory
{
    public static ILoanCalculationStrategy GetStrategy(LoanType loanType) => loanType switch
    {
        LoanType.Fixed => new FixedLoanStrategy(),
        LoanType.Decreasing => new DecreasingLoanStrategy(),
        _ => throw new ArgumentException($"Tipo de préstamo no soportado: {loanType}")
    };
}
