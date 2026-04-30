using System.Security.Claims;
using BankingApi.DTOs.Transaction;
using BankingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Όλα τα endpoints απαιτούν JWT token
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    private int GetCustomerId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetByAccountId(int accountId)
    {
        var transactions = await _transactionService.GetByAccountIdAsync(accountId, GetCustomerId());
        return Ok(transactions);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositDto dto)
    {
        var transaction = await _transactionService.DepositAsync(dto, GetCustomerId());
        return Ok(transaction);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] WithdrawDto dto)
    {
        var transaction = await _transactionService.WithdrawAsync(dto, GetCustomerId());
        return Ok(transaction);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferDto dto)
    {
        var transaction = await _transactionService.TransferAsync(dto, GetCustomerId());
        return Ok(transaction);
    }
}