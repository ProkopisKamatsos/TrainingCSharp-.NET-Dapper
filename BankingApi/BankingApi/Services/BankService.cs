using BankingApi.Contracts;
using BankingApi.Domain;

namespace BankingApi.Services;

public class BankService : IBankService
{
    private readonly IBankAccountRepository _repository;

    public BankService(IBankAccountRepository repository)
    {
        _repository = repository;
    }

    public BankAccount CreateAccount()
    {
        return _repository.Create();
    }

    public BankAccount GetAccount(int id)
    {
        var account = _repository.GetById(id);
        if (account is null)
            throw new KeyNotFoundException("Account not found.");

        return account;
    }

    public BankAccount Deposit(int id, decimal amount)
    {
        var account = GetAccount(id);
        account.Deposit(amount);
        _repository.Save(account);
        return account;
    }

    public BankAccount Withdraw(int id, decimal amount)
    {
        var account = GetAccount(id);
        account.Withdraw(amount);
        _repository.Save(account);
        return account;
    }
}
