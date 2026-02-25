using Dapper;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IDbConnectionFactory _db;

    public TaskRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<int> CreateAsync(CreateTaskDto dto, DateTime? completedAt)
    {
        const string sql = """
        INSERT INTO Tasks (Title, Description, Status, Priority, UserId, DueDate, CompletedAt)
        OUTPUT INSERTED.Id
        VALUES (@Title, @Description, @Status, @Priority, @UserId, @DueDate, @CompletedAt);
        """;

        using var conn = _db.CreateConnection();

        return await conn.ExecuteScalarAsync<int>(sql, new
        {
            dto.Title,
            dto.Description,
            dto.Status,
            dto.Priority,
            dto.UserId,
            dto.DueDate,
            CompletedAt = completedAt
        });
    }

    public async Task<TaskDto?> GetByIdAsync(int id)
    {
        const string sql = """
            SELECT
                Id AS TaskId,
                Title,
                Description,
                Status,
                Priority,
                UserId,
                DueDate,
                CreatedAt,
                CompletedAt
            FROM Tasks
            WHERE Id = @Id;
            """;

        using var conn = _db.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<TaskDto>(sql, new { Id = id });
    }
    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto, DateTime? completedAt)
    {
        const string sql = """
        UPDATE Tasks
        SET Title = @Title,
            Description = @Description,
            Status = @Status,
            Priority = @Priority,
            DueDate = @DueDate,
            CompletedAt = @CompletedAt,
            UpdatedAt = GETDATE()
        WHERE Id = @Id;
        """;

        using var conn = _db.CreateConnection();

        var rows = await conn.ExecuteAsync(sql, new
        {
            Id = id,
            dto.Title,
            dto.Description,
            dto.Status,
            dto.Priority,
            dto.DueDate,
            CompletedAt = completedAt
        });

        return rows > 0;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Tasks WHERE Id = @Id;";

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
    public async Task<IEnumerable<TaskDto>> GetByUserIdAsync(int userId)
    {
        const string sql = """
        SELECT
            Id AS TaskId,
            Title,
            Description,
            Status,
            Priority,
            UserId,
            DueDate,
            CreatedAt,
            CompletedAt
        FROM Tasks
        WHERE UserId = @UserId
        ORDER BY CreatedAt DESC;
        """;

        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<TaskDto>(sql, new { UserId = userId });
    }
    public async Task<IEnumerable<TaskDto>> GetByStatusAsync(string status)
    {
        const string sql = """
        SELECT
            Id AS TaskId,
            Title,
            Description,
            Status,
            Priority,
            UserId,
            DueDate,
            CreatedAt,
            CompletedAt
        FROM Tasks
        WHERE Status = @Status
        ORDER BY CreatedAt DESC;
        """;

        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<TaskDto>(sql, new { Status = status });
    }
    public async Task<IEnumerable<TaskDto>> SearchAsync(string keyword)
    {
        const string sql = """
        SELECT
            Id AS TaskId,
            Title,
            Description,
            Status,
            Priority,
            UserId,
            DueDate,
            CreatedAt,
            CompletedAt
        FROM Tasks
        WHERE Title LIKE @Pattern OR Description LIKE @Pattern
        ORDER BY CreatedAt DESC;
        """;

        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<TaskDto>(sql, new { Pattern = $"%{keyword}%" });
    }




}
