using Dapper;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly SqlConnectionFactory _connectionFactory;

    public TaskRepository(SqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(TaskItem task)
    {
        const string sql = @"
            INSERT INTO Tasks
            (
                Title,
                Description,
                Status,
                Priority,
                UserId,
                DueDate,
                CreatedAt,
                UpdatedAt,
                CompletedAt
            )
            VALUES
            (
                @Title,
                @Description,
                @Status,
                @Priority,
                @UserId,
                @DueDate,
                @CreatedAt,
                @UpdatedAt,
                @CompletedAt
            );

            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, task);
    }

    public async Task<List<TaskItem>> GetByUserIdAsync(int userId)
    {
        const string sql = @"
            SELECT Id, Title, Description, Status, Priority, UserId, DueDate, CreatedAt, UpdatedAt, CompletedAt
            FROM Tasks
            WHERE UserId = @UserId
            ORDER BY CreatedAt DESC";

        using var connection = _connectionFactory.CreateConnection();
        var tasks = await connection.QueryAsync<TaskItem>(sql, new { UserId = userId });

        return tasks.ToList();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT Id, Title, Description, Status, Priority, UserId, DueDate, CreatedAt, UpdatedAt, CompletedAt
            FROM Tasks
            WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TaskItem>(sql, new { Id = id });
    }
    public async Task<bool> UpdateAsync(TaskItem task)
    {
        const string sql = @"
        UPDATE Tasks
        SET Title = @Title,
            Description = @Description,
            Status = @Status,
            Priority = @Priority,
            DueDate = @DueDate,
            UpdatedAt = @UpdatedAt,
            CompletedAt = @CompletedAt
        WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, task);

        return rowsAffected > 0;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = @"
        DELETE FROM Tasks
        WHERE Id = @Id";

        using var connection = _connectionFactory.CreateConnection();
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

        return rowsAffected > 0;
    }
}