using Dapper;
using Microsoft.Data.SqlClient;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;

namespace BankingApi.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public TransactionRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Transaction>> GetByAccountIdAsync(int accountId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            SELECT * FROM Transactions
            WHERE FromAccountId = @AccountId OR ToAccountId = @AccountId
            ORDER BY CreatedAt DESC";

        return await connection.QueryAsync<Transaction>(sql, new { AccountId = accountId });
    }
    public async Task<Transaction?> GetByIdAsync(int transactionId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";
        return await connection.QuerySingleOrDefaultAsync<Transaction>(sql, new { TransactionId = transactionId });
    }
    public async Task<int> CreateAsync(Transaction transaction, SqlConnection connection, SqlTransaction sqlTransaction)
    {
        const string sql = @"
            INSERT INTO Transactions (FromAccountId, ToAccountId, Amount, TransactionType, Status, Description)
            VALUES (@FromAccountId, @ToAccountId, @Amount, @TransactionType, @Status, @Description);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        return await connection.ExecuteScalarAsync<int>(sql, transaction, sqlTransaction);
    }
}