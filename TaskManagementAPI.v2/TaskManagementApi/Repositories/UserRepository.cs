using Dapper;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public UserRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT Id, Username, Email, PasswordHash, FirstName, LastName, Role, CreatedAt, UpdatedAt, IsActive
            FROM Users
            WHERE Email = @Email";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = @"
            SELECT Id, Username, Email, PasswordHash, FirstName, LastName, Role, CreatedAt, UpdatedAt, IsActive
            FROM Users
            WHERE Username = @Username";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, Username, Email, PasswordHash, FirstName, LastName, Role, CreatedAt, UpdatedAt, IsActive
            FROM Users
            WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, Role, CreatedAt, UpdatedAt, IsActive)
            VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @Role, @CreatedAt, @UpdatedAt, @IsActive);

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(sql, user);
    }
    public async Task<bool> UpdateAsync(User user)
    {
        const string sql = @"
        UPDATE Users
        SET Username = @Username,
            Email = @Email,
            FirstName = @FirstName,
            LastName = @LastName,
            UpdatedAt = @UpdatedAt
        WHERE Id = @Id AND IsActive = 1";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, user);

        return rowsAffected > 0;
    }
    public async Task<bool> DeactivateAsync(int id)
    {
        const string sql = @"
        UPDATE Users
        SET IsActive = 0,
            UpdatedAt = GETDATE()
        WHERE Id = @Id AND IsActive = 1";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

        return rowsAffected > 0;
    }
}