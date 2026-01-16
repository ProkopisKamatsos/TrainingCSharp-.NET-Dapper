using BankingApi.Contracts;
using BankingApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly IBankService _bankService;

    public AccountsController(IBankService bankService)
    {
        _bankService = bankService;
    }

    [HttpPost]
    public ActionResult<AccountResponse> Create()
    {
        var account = _bankService.CreateAccount();
        return Ok(new AccountResponse(account.Id, account.Balance));
    }

    [HttpGet("{id:int}")]
    public ActionResult<AccountResponse> GetById(int id)
    {
        var account = _bankService.GetAccount(id);
        return Ok(new AccountResponse(account.Id, account.Balance));
    }

    [HttpPost("{id:int}/deposit")]
    public ActionResult<AccountResponse> Deposit(int id, MoneyRequest request)
    {
        var account = _bankService.Deposit(id, request.Amount);
        return Ok(new AccountResponse(account.Id, account.Balance));
    }

    [HttpPost("{id:int}/withdraw")]
    public ActionResult<AccountResponse> Withdraw(int id, MoneyRequest request)
    {
        var account = _bankService.Withdraw(id, request.Amount);
        return Ok(new AccountResponse(account.Id, account.Balance));
    }
}
