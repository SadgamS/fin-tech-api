using FinTech.Application.DTOs.Transaction;
using FinTech.Application.Services;
using FinTech.Domain.Interfaces;
using FinTech.Infrastructure.Data;
using FinTech.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FinTech.Tests.Integration;

public class TransactionIdempotencyTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly ITransactionRepository _repo;
    private readonly TransactionService _service;

    public void Dispose()
    {
        _context.Dispose();
    }

    public TransactionIdempotencyTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repo = new TransactionRepository(_context);
        _service = new TransactionService(_repo);
    }

    [Fact]
    public async Task CreateTransaction_SameIdempotencyKey_ShouldReturnOriginalTransaction()
    {
        // Arrange
        var request = new CreateTransactionRequest
        {
            IdempotencyKey = "test-key-001",
            Type = "Payment",
            Amount = 500m,
            Description = "Pago cuota 1"
        };
         
        // Act
        var (first, firstIsNew) = await _service.CreateAsync(request);
        var (second, secondIsNew) = await _service.CreateAsync(request);

        // Assert
        firstIsNew.Should().BeTrue();
        secondIsNew.Should().BeFalse();
        first.Id.Should().Be(second.Id);
        first.IdempotencyKey.Should().Be(second.IdempotencyKey);
    }

    [Fact]
    public async Task CreateTransaction_DifferentIdempotencyKeys_ShouldCreateTwoTransactions()
    {
        // Arrange
        var request1 = new CreateTransactionRequest
        {
            IdempotencyKey = "key-001",
            Type = "Payment",
            Amount = 500m
        };
        var request2 = new CreateTransactionRequest
        {
            IdempotencyKey = "key-002",
            Type = "Payment",
            Amount = 500m
        };

        // Act
        var (first, firstIsNew) = await _service.CreateAsync(request1);
        var (second, secondIsNew) = await _service.CreateAsync(request2);

        // Assert
        firstIsNew.Should().BeTrue();
        secondIsNew.Should().BeTrue();
        first.Id.Should().NotBe(second.Id);
    }
}
