using BankingApi.Models;
using Microsoft.Data.SqlClient;

namespace BankingApi.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetByCustomerIdAsync(int customerId);
    Task<Account?> GetByIdAsync(int accountId);
    Task<int> CreateAsync(Account account);
    Task UpdateBalanceAsync(int accountId, decimal newBalance);
    Task UpdateBalanceAsync(int accountId, decimal newBalance, SqlConnection connection, SqlTransaction sqlTransaction);
    Task DeactivateAsync(int accountId);
}