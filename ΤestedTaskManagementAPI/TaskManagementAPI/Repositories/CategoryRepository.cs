using Dapper;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IDbConnectionFactory _db;

    public CategoryRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        const string sql = """
            SELECT Id, Name, Color, Icon
            FROM Categories
            ORDER BY Name;
            """;

        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<CategoryDto>(sql);
    }

    public async Task<int> CreateAsync(CreateCategoryDto dto)
    {
        const string sql = """
            INSERT INTO Categories (Name, Color, Icon)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Color, @Icon);
            """;

        using var conn = _db.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(sql, dto);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        const string sql = """
            UPDATE Categories
            SET Name = @Name, Color = @Color, Icon = @Icon
            WHERE Id = @Id;
            """;

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id, dto.Name, dto.Color, dto.Icon });
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Categories WHERE Id = @Id;";

        using var conn = _db.CreateConnection();
        var rows = await conn.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
