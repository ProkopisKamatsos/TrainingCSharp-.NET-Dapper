using Dapper;
using WebApplication1.Data;
namespace WebApplication1.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly DbContext _db;
        public TodoRepository(DbContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<TodoDTO>> GetAllAsync()
        {
            using var conn = _db.CreateConnection();
            var sql = "SELECT * FROM Todo";
            return await conn.QueryAsync<TodoDTO>(sql);
        }
        public async Task<TodoDTO> CreateAsync(CreateTodoDTO dto)
        {
            const string sql = """
            INSERT INTO Todo (Name, Secret)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Secret);
            """;

            using var connection = _db.CreateConnection();

            var newId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                dto.Name,
                dto.Secret
            });

            return new TodoDTO
            {
                Id = newId,
                Name = dto.Name
            };
        }

        public async Task<TodoDTO?> GetByIdAsync(int id)
        {
            const string sql = """
            SELECT Id, Name
            FROM dbo.Todo
            WHERE Id = @Id;
            """;

            using var connection = _db.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<TodoDTO>(sql, new { Id = id });
        }
        public async Task<bool> UpdateAsync(int id, UpdateTodoDTO dto)
        {
            const string sql = """
            UPDATE dbo.Todo
            SET Name = @Name
            WHERE Id = @Id;
            """;

            using var connection = _db.CreateConnection();

            var rows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                Name = dto.Name
            });

            return rows > 0;

        }

        public async Task<bool> DeleteAsync(int id)
        {

            const string sql = """
             DELETE FROM dbo.Todo
             WHERE Id = @Id;
            """;
            using var connection = _db.CreateConnection();

            var rows = await connection.ExecuteAsync(sql, new { Id = id });

            return rows > 0; 
        }
    }
}
