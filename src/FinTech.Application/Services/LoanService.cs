using FinTech.Application.DTOs.Loan;
using FinTech.Application.DTOs.Transaction;
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FinTech.Domain.Interfaces;

namespace FinTech.Application.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepo;
    private readonly ITransactionService _transactionService;

    public LoanService(ILoanRepository loanRepo, ITransactionService transactionService)
    {
        _loanRepo = loanRepo;
        _transactionService = transactionService;
    }

    public async Task<LoanResponse> ApproveAsync(Guid id)
    {
        var loan = await _loanRepo.GetByIdAsync(id);
        if (loan == null) throw new KeyNotFoundException("Préstamo no encontrado");
        if (loan.Status != LoanStatus.Pending)
            throw new InvalidOperationException("Solo se pueden aprobar préstamos en estado Pending");

        loan.Status = LoanStatus.Active;
        await _loanRepo.UpdateAsync(loan);
        await CreateDisbursementTransaction(loan);
        return MapLoan(loan);
    }

    public async Task<LoanResponse> CreateAsync(CreateLoanRequest request)
    {
        if (!Enum.TryParse<LoanType>(request.LoanType, true, out var loanType))
            throw new ArgumentException("Tipo de préstamo inválido");

        var activeCount = await _loanRepo.CountActiveByUserAsync(request.UserId);
        var existingPayments = await _loanRepo.SumMonthlyPaymentsByUserAsync(request.UserId);

        var loan = new Loan(
            userId: request.UserId,
            amount: request.Amount,
            term: request.Term,
            interestRate: request.InterestRate,
            loanType: loanType,
            monthlyIncome: request.MonthlyIncome
        );

        loan.ValidateRules(activeCount, existingPayments);
        loan.ApplyAutoScoring(activeCount);
        loan.GenerateSchedule(DateTime.UtcNow);

        await _loanRepo.CreateAsync(loan);

        if (loan.Status == LoanStatus.Approved)
        {
            loan.Status = LoanStatus.Active;
            await _loanRepo.UpdateAsync(loan);
            await CreateDisbursementTransaction(loan);
        }

        return MapLoan(loan);

    }

    public async Task<IEnumerable<LoanResponse>> GetAllAsync(string? userId = null)
    {
        var loans = await _loanRepo.GetAllAsync(userId);
        return loans.Select(MapLoan);
    }

    public async Task<LoanResponse?> GetByIdAsync(Guid id)
    {
        var loan = await _loanRepo.GetByIdWithScheduleAsync(id);
        return loan == null ? null : MapLoan(loan);
    }

    public async Task<List<PaymentScheduleResponse>> GetScheduleAsync(Guid id)
    {
        var loan = await _loanRepo.GetByIdWithScheduleAsync(id);
        if (loan == null) throw new KeyNotFoundException("Préstamo no encontrado");
        return loan.PaymentSchedules
            .OrderBy(p => p.PaymentNumber)
            .Select(MapSchedule)
            .ToList();
    }

    public async Task<LoanResponse> RejectAsync(Guid id)
    {
        var loan = await _loanRepo.GetByIdAsync(id);
        if (loan == null) throw new KeyNotFoundException("Préstamo no encontrado");
        if (loan.Status != LoanStatus.Pending)
            throw new InvalidOperationException("Solo se pueden rechazar préstamos en estado Pending");

        loan.Status = LoanStatus.Rejected;
        await _loanRepo.UpdateAsync(loan);
        return MapLoan(loan);
    }

    private async Task CreateDisbursementTransaction(Loan loan) =>
    await _transactionService.CreateAsync(new CreateTransactionRequest
    {
        IdempotencyKey = $"disbursement-{loan.Id}",
        Type = "Disbursement",
        Amount = loan.Amount,
        LoanId = loan.Id,
        Description = $"Desembolso préstamo {loan.Id}"
    });

    public Task<SimulateLoanResponse> SimulateAsync(SimulateLoanRequest request)
    {
        if (!Enum.TryParse<LoanType>(request.LoanType, true, out var loanType))
            throw new ArgumentException("Tipo de préstamo inválido");

        var tempLoan = new Loan(
            userId: "temp",
            amount: request.Amount,
            term: request.Term,
            interestRate: request.InterestRate,
            loanType: loanType,
            monthlyIncome: request.MonthlyIncome
        );

        tempLoan.GenerateSchedule(DateTime.UtcNow);
        var totalPayment = tempLoan.PaymentSchedules.Sum(s => s.TotalPayment);

        return Task.FromResult(new SimulateLoanResponse
        {
            Amount = tempLoan.Amount,
            Term = tempLoan.Term,
            InterestRate = tempLoan.InterestRate,
            MonthlyPayment = tempLoan.MonthlyPayment,
            TotalPayment = totalPayment,
            TotalInterest = Math.Round(totalPayment - request.Amount, 2),
            Schedule = tempLoan.PaymentSchedules.Select(MapSchedule).ToList()
        });

    }

    private static PaymentScheduleResponse MapSchedule(PaymentSchedule s) => new()
    {
        Id = s.Id,
        PaymentNumber = s.PaymentNumber,
        DueDate = s.DueDate,
        TotalPayment = s.TotalPayment,
        Principal = s.Principal,
        Interest = s.Interest,
        RemainingBalance = s.RemainingBalance,
        Status = s.Status.ToString()
    };

    private static LoanResponse MapLoan(Loan loan) => new()
    {
        Id = loan.Id,
        UserId = loan.UserId,
        Amount = loan.Amount,
        Term = loan.Term,
        InterestRate = loan.InterestRate,
        LoanType = loan.LoanType.ToString(),
        Status = loan.Status.ToString(),
        MonthlyPayment = loan.MonthlyPayment,
        MonthlyIncome = loan.MonthlyIncome,
        CreatedAt = loan.CreatedAt,
        UpdatedAt = loan.UpdatedAt.GetValueOrDefault(),
        PaymentSchedules = loan.PaymentSchedules
        .OrderBy(p => p.PaymentNumber)
        .Select(MapSchedule)
        .ToList()
    };
}
