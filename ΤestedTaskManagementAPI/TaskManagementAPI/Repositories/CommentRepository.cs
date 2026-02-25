using Dapper;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly IDbConnectionFactory _db;

    public CommentRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CommentDto>> GetByTaskIdAsync(int taskId)
    {
        const string sql = """
            SELECT Id, TaskId, UserId, Content, CreatedAt
            FROM Comments
            WHERE TaskId = @TaskId
            ORDER BY CreatedAt ASC;
            """;

        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<CommentDto>(sql, new { TaskId = taskId });
    }

    public async Task<int> CreateAsync(int taskId, CreateCommentDto dto)
    {
        const string sql = """
            INSERT INTO Comments (TaskId, UserId, Content)
            OUTPUT INSERTED.Id
            VALUES (@TaskId, @UserId, @Content);
            """;

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql, new { TaskId = taskId, dto.UserId, dto.Content });
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Comments WHERE Id = @Id;";

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
