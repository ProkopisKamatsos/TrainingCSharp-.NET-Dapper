using Dapper;
using Microsoft.Data.SqlClient;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;

namespace BankingApi.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public AccountRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<Account>> GetByCustomerIdAsync(int customerId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            SELECT * FROM Accounts 
            WHERE CustomerId = @CustomerId AND IsActive = 1";

        return await connection.QueryAsync<Account>(sql, new { CustomerId = customerId });
    }

    public async Task<Account?> GetByIdAsync(int accountId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            SELECT * FROM Accounts 
            WHERE AccountId = @AccountId AND IsActive = 1";

        return await connection.QuerySingleOrDefaultAsync<Account>(sql, new { AccountId = accountId });
    }

    public async Task<int> CreateAsync(Account account)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            INSERT INTO Accounts (CustomerId, AccountNumber, AccountType, Balance)
            VALUES (@CustomerId, @AccountNumber, @AccountType, @Balance);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        return await connection.ExecuteScalarAsync<int>(sql, account);
    }

    public async Task CreditAsync(int accountId, decimal amount, SqlConnection connection, SqlTransaction sqlTransaction)
    {
        const string sql = @"
            UPDATE Accounts
            SET Balance = Balance + @Amount, UpdatedAt = GETUTCDATE()
            WHERE AccountId = @AccountId AND IsActive = 1";

        await connection.ExecuteAsync(sql, new { AccountId = accountId, Amount = amount }, sqlTransaction);
    }

    public async Task DebitAsync(int accountId, decimal amount, SqlConnection connection, SqlTransaction sqlTransaction)
    {
        const string sql = @"
            UPDATE Accounts
            SET Balance = Balance - @Amount, UpdatedAt = GETUTCDATE()
            WHERE AccountId = @AccountId AND Balance >= @Amount AND IsActive = 1";

        var rows = await connection.ExecuteAsync(sql, new { AccountId = accountId, Amount = amount }, sqlTransaction);

        if (rows == 0)
            throw new InvalidOperationException("Insufficient funds or account not found.");
    }

    public async Task DeactivateAsync(int accountId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            UPDATE Accounts 
            SET IsActive = 0, UpdatedAt = GETUTCDATE()
            WHERE AccountId = @AccountId";

        await connection.ExecuteAsync(sql, new { AccountId = accountId });
    }
}