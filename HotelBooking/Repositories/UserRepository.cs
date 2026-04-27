using Dapper;
using HotelBooking.Data;
using HotelBooking.Models;

namespace HotelBooking.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            const string sql = @"
                SELECT Id, FullName, Email, PasswordHash, Role, IsActive, CreatedAt
                FROM Users
                WHERE Email = @Email AND IsActive = 1";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<int> CreateAsync(User user)
        {
            const string sql = @"
                INSERT INTO Users (FullName, Email, PasswordHash, Role, IsActive, CreatedAt)
                VALUES (@FullName, @Email, @PasswordHash, @Role, 1, GETUTCDATE());
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }
    }
}