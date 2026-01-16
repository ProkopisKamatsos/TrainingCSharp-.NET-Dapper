using BankingApi.Contracts;
using BankingApi.Domain;

namespace BankingApi.Repositories;

public class InMemoryBankAccountRepository : IBankAccountRepository
{
    private readonly Dictionary<int, BankAccount> _store = new();
    private int _nextId = 1;

    public BankAccount Create()
    {
        var id = _nextId++;
        var account = new BankAccount(id);
        _store[id] = account;
        return account;
    }

    public BankAccount? GetById(int id)
    {
        _store.TryGetValue(id, out var account);
        return account;
    }

    public void Save(BankAccount account)
    {
        _store[account.Id] = account;
    }
}
