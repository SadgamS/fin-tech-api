using FinTech.Application.DTOs.Loan;
using FinTech.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers;

[ApiController]
[Route("api/loans")]
[Produces("application/json")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost("simulate")]
    [ProducesResponseType(typeof(SimulateLoanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimulateLoanResponse>> Simulate([FromBody] SimulateLoanRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var result = await _loanService.SimulateAsync(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(LoanResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoanResponse>> Create([FromBody] CreateLoanRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var loan = await _loanService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
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

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LoanResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<LoanResponse>>> GetAll([FromQuery] string? userId)
    {
        var loans = await _loanService.GetAllAsync(userId);
        return Ok(loans);
    }

    /// <summary>Obtiene un préstamo por ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LoanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoanResponse>> GetById(Guid id)
    {
        var loan = await _loanService.GetByIdAsync(id);
        if (loan is null) return NotFound(new { message = "Préstamo no encontrado" });
        return Ok(loan);
    }

    /// <summary>Obtiene el cronograma de pagos de un préstamo</summary>
    [HttpGet("{id:guid}/schedule")]
    [ProducesResponseType(typeof(IEnumerable<PaymentScheduleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<PaymentScheduleResponse>>> GetSchedule(Guid id)
    {
        try
        {
            var schedule = await _loanService.GetScheduleAsync(id);
            return Ok(schedule);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>Aprueba un préstamo (cambia estado a Active y crea transacción de desembolso)</summary>
    [HttpPatch("{id:guid}/approve")]
    [ProducesResponseType(typeof(LoanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoanResponse>> Approve(Guid id)
    {
        try
        {
            var loan = await _loanService.ApproveAsync(id);
            return Ok(loan);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Rechaza un préstamo</summary>
    [HttpPatch("{id:guid}/reject")]
    [ProducesResponseType(typeof(LoanResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LoanResponse>> Reject(Guid id)
    {
        try
        {
            var loan = await _loanService.RejectAsync(id);
            return Ok(loan);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
