using BankingApi.Models;
using Microsoft.Data.SqlClient;

namespace BankingApi.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId);
    Task<Transaction?> GetByIdAsync(int transactionId);
    Task<int> CreateAsync(Transaction transaction, SqlConnection connection, SqlTransaction sqlTransaction);
}