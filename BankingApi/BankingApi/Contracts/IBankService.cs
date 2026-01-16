using BankingApi.Domain;

namespace BankingApi.Contracts;

public interface IBankService
{
    BankAccount CreateAccount();
    BankAccount GetAccount(int id);
    BankAccount Deposit(int id, decimal amount);
    BankAccount Withdraw(int id, decimal amount);
}
