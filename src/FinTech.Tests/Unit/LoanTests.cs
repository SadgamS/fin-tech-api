using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FluentAssertions;

namespace FinTech.Tests.Unit;

public class LoanTests
{
    [Fact]
    public void CalculateMonthlyPayment_FixedLoan_ShouldReturnCorrectPayment()
    {
        // Arrange & Act
        var loan = new Loan(
            userId: "user-001",
            amount: 10000m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Assert
        loan.MonthlyPayment.Should().BeApproximately(934.53m, 1m);
    }

    [Fact]
    public void GenerateSchedule_FixedLoan_ShouldGenerateCorrectNumberOfPayments()
    {
        // Arrange
        var loan = new Loan(
            userId: "user-001",
            amount: 10000m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Act
        loan.GenerateSchedule(DateTime.UtcNow);

        // Assert
        loan.PaymentSchedules.Should().HaveCount(12);
    }

    [Fact]
    public void GenerateSchedule_FixedLoan_LastPaymentShouldHaveZeroBalance()
    {
        // Arrange
        var loan = new Loan(
            userId: "user-001",
            amount: 10000m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Act
        loan.GenerateSchedule(DateTime.UtcNow);

        // Assert
        var lastPayment = loan.PaymentSchedules.OrderBy(p => p.PaymentNumber).Last();
        lastPayment.RemainingBalance.Should().BeApproximately(0m, 0.01m);
    }


    [Fact]
    public void CreateLoan_AmountBelowMinimum_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => new Loan(
            userId: "user-001",
            amount: 100m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("amount");
    }

    [Fact]
    public void CreateLoan_AmountAboveMaximum_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => new Loan(
            userId: "user-001",
            amount: 100000m,
            term: 12,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("amount");
    }

    [Fact]
    public void CreateLoan_TermBelowMinimum_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => new Loan(
            userId: "user-001",
            amount: 5000m,
            term: 3,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("term");
    }

    [Fact]
    public void CreateLoan_TermAboveMaximum_ShouldThrowException()
    {
        // Arrange & Act
        var act = () => new Loan(
            userId: "user-001",
            amount: 5000m,
            term: 72,
            interestRate: 0.24m,
            loanType: LoanType.Fixed,
            monthlyIncome: 5000m
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("term");
    }


}
