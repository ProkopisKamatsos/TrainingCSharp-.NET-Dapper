using BankingApi.Contracts;
using BankingApi.Data;
using BankingApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BankingApi.Repositories;

public class EfBankAccountRepository : IBankAccountRepository
{
    private readonly BankingDbContext _db;

    public EfBankAccountRepository(BankingDbContext db)
    {
        _db = db;
    }

    public BankAccount Create()
    {
        // αφήνουμε τη DB να δώσει Id (auto-increment)
        var account = new BankAccount(id: 0);

        _db.BankAccounts.Add(account);
        _db.SaveChanges();

        return account;
    }

    public BankAccount? GetById(int id)
    {
        return _db.BankAccounts.FirstOrDefault(a => a.Id == id);
    }

    public void Save(BankAccount account)
    {
        // Το account είναι tracked αν το πήραμε από _db
        // Για ασφάλεια, λέμε Update και κάνουμε SaveChanges
        _db.BankAccounts.Update(account);
        _db.SaveChanges();
    }
}
