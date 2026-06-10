using FinTech.Application.DTOs.Transaction;
using FinTech.Application.Interfaces;
using FinTech.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Crea una transacción con garantía de idempotencia.
    /// Si se envía el mismo IdempotencyKey dos veces, retorna la transacción original.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionResponse>> Create([FromBody] CreateTransactionRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var (transaction, isNew) = await _transactionService.CreateAsync(request);

            if (isNew)
                return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);

            // Mismo idempotency_key → retorna la original con 200 OK (no duplica)
            return Ok(transaction);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Lista transacciones con filtros opcionales por tipo y estado</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TransactionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TransactionResponse>>> GetAll(
        [FromQuery] string? type,
        [FromQuery] string? status)
    {
        TransactionType? parsedType = null;
        TransactionStatus? parsedStatus = null;

        if (!string.IsNullOrEmpty(type))
        {
            if (!Enum.TryParse<TransactionType>(type, true, out var t))
                return BadRequest(new { message = $"Tipo inválido. Valores válidos: {string.Join(", ", Enum.GetNames<TransactionType>())}" });
            parsedType = t;
        }

        if (!string.IsNullOrEmpty(status))
        {
            if (!Enum.TryParse<TransactionStatus>(status, true, out var s))
                return BadRequest(new { message = $"Estado inválido. Valores válidos: {string.Join(", ", Enum.GetNames<TransactionStatus>())}" });
            parsedStatus = s;
        }

        var transactions = await _transactionService.GetAllAsync(parsedType, parsedStatus);
        return Ok(transactions);
    }

    /// <summary>Obtiene una transacción por ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TransactionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionResponse>> GetById(Guid id)
    {
        var transaction = await _transactionService.GetByIdAsync(id);
        if (transaction is null) return NotFound(new { message = "Transacción no encontrada" });
        return Ok(transaction);
    }

}
