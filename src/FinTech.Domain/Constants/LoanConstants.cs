namespace FinTech.Domain.Constants;

public static class LoanConstants
{
    public const decimal MinimumAmount = 500m;
    public const decimal MaximumAmount = 50000m;
    public const int MinimumTermInMonths = 6;
    public const int MaximumTermInMonths = 60;
    public const int MaxActiveLoans = 3;
    public const decimal MaxPaymentIncomeRatio = 0.40m;

}
