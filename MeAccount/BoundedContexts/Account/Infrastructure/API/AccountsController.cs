using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MeAccount.BoundedContexts.Account.Application.DTOs;
using MeAccount.BoundedContexts.Account.Application.Services;
using MeAccount.SharedKernel.Infrastructure;

namespace MeAccount.BoundedContexts.Account.Infrastructure.API;

[Authorize]
public class AccountsController : BaseApiController
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var accounts = await _accountService.GetAllAsync(GetUserId());
            return Ok(accounts);
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
            var account = await _accountService.GetByIdAsync(id, GetUserId());
            return Ok(account);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        try
        {
            var account = await _accountService.CreateAsync(dto, GetUserId());
            return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAccountDto dto)
    {
        try
        {
            var account = await _accountService.UpdateAsync(id, dto, GetUserId());
            return Ok(account);
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
            await _accountService.DeleteAsync(id, GetUserId());
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
