using BankingApi.Domain;

namespace BankingApi.Contracts;

public interface IBankAccountRepository
{
    BankAccount Create();
    BankAccount? GetById(int id);
    void Save(BankAccount account);
}
