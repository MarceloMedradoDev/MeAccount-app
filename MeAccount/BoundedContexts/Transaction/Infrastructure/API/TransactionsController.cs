using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeAccount.BoundedContexts.Transaction.Application.DTOs;
using MeAccount.BoundedContexts.Transaction.Application.Services;
using MeAccount.SharedKernel.Infrastructure;

namespace MeAccount.BoundedContexts.Transaction.Infrastructure.API;

[Authorize]
public class TransactionsController : BaseApiController
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] Guid? accountId,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? type)
    {
        try
        {
            var transactions = await _transactionService.GetAllAsync(
                GetUserId(), startDate, endDate, accountId, categoryId, type);
            return Ok(transactions);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var transaction = await _transactionService.GetByIdAsync(id, GetUserId());
            return Ok(transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        try
        {
            var transaction = await _transactionService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionDto dto)
    {
        try
        {
            var transaction = await _transactionService.UpdateAsync(id, dto, GetUserId());
            return Ok(transaction);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _transactionService.DeleteAsync(id, GetUserId());
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
