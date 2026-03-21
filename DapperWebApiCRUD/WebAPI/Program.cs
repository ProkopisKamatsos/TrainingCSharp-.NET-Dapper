using Dapper;
using Microsoft.Data.SqlClient;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/users", async (IConfiguration configuration) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "SELECT * FROM Users;";
    var users = await connection.QueryAsync<User>(sql);
    return Results.Ok(users);
});
app.MapGet("/users/{id}", async (IConfiguration configuration, int id) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "SELECT * FROM Users WHERE Id = @Id;";
    var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    return user is not null ? Results.Ok(user) : Results.NotFound();
});
app.MapPost("/users", async (IConfiguration configuration, User user) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email); SELECT CAST(SCOPE_IDENTITY() as int);";
    var id = await connection.QuerySingleAsync<int>(sql, user);
    user.Id = id;
    return Results.Created($"/users/{id}", user);
});
app.MapPut("/users/{id}", async (IConfiguration configuration, int id, User user) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "UPDATE Users SET Username = @Username, Email = @Email WHERE Id = @Id;";
    var affectedRows = await connection.ExecuteAsync(sql, new { user.Username, user.Email, Id = id });
    return affectedRows > 0 ? Results.NoContent() : Results.NotFound();
});
app.MapDelete("/users/{id}", async (IConfiguration configuration, int id) =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    using var connection = new SqlConnection(connectionString);
    const string sql = "DELETE FROM Users WHERE Id = @Id;";
    var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
    return affectedRows > 0 ? Results.NoContent() : Results.NotFound();
});
app.UseHttpsRedirection();

app.Run();

