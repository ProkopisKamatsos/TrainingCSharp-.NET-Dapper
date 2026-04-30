using Dapper;
using BankingApi.Data;
using BankingApi.Models;
using BankingApi.Repositories.Interfaces;

namespace BankingApi.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public CustomerRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<Customer?> GetByIdAsync(int customerId)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            SELECT * FROM Customers 
            WHERE CustomerId = @CustomerId AND IsActive = 1";

        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { CustomerId = customerId });
    }

    public async Task<Customer?> GetByEmailAsync(string email)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            SELECT * FROM Customers 
            WHERE Email = @Email AND IsActive = 1";

        return await connection.QuerySingleOrDefaultAsync<Customer>(sql, new { Email = email });
    }

    public async Task<int> CreateAsync(Customer customer)
    {
        using var connection = _connectionFactory.Create();
        const string sql = @"
            INSERT INTO Customers (FirstName, LastName, Email, PasswordHash, PhoneNumber)
            VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PhoneNumber);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        return await connection.ExecuteScalarAsync<int>(sql, customer);
    }
}