using System.Security.Claims;
using BankingApi.DTOs.Account;
using BankingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Όλα τα endpoints απαιτούν JWT token
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // Παίρνουμε το CustomerId από το JWT token — όχι από το URL
    // Έτσι ο χρήστης δεν μπορεί να δει accounts άλλου customer
    private int GetCustomerId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyAccounts()
    {
        var accounts = await _accountService.GetMyAccountsAsync(GetCustomerId());
        return Ok(accounts);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetById(int accountId)
    {
        var account = await _accountService.GetByIdAsync(accountId, GetCustomerId());
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountDto dto)
    {
        var account = await _accountService.CreateAsync(dto, GetCustomerId());
        return CreatedAtAction(nameof(GetById), new { accountId = account.AccountId }, account);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> Deactivate(int accountId)
    {
        await _accountService.DeactivateAsync(accountId, GetCustomerId());
        return NoContent();
    }
}