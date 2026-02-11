using Dapper;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _db;

    public UserRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql = """
            INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName)
            OUTPUT INSERTED.Id
            VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName);
            """;

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = "SELECT * FROM Users WHERE Id = @Id AND IsActive = 1";
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        const string sql = "SELECT * FROM Users WHERE Email = @Email";
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = "SELECT * FROM Users WHERE Username = @Username";
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }
    public async Task<bool> UpdateProfileAsync(int id, string? firstName, string? lastName)
    {
        const string sql = """
        UPDATE Users
        SET FirstName = @FirstName,
            LastName = @LastName,
            UpdatedAt = GETDATE()
        WHERE Id = @Id AND IsActive = 1;
        """;

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id, FirstName = firstName, LastName = lastName });
        return rows > 0;
    }
    public async Task<bool> DeactivateAsync(int id)
    {
        const string sql = """
        UPDATE Users
        SET IsActive = 0,
            UpdatedAt = GETDATE()
        WHERE Id = @Id AND IsActive = 1;
        """;

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }




}
