using DemoCrudDapperADONET;
using Dapper;
using Microsoft.Data.SqlClient;

const string cs ="Server=localhost\\SQLEXPRESS;Database=AdoNetPractice;Trusted_Connection=True;TrustServerCertificate=True;";
using var conn = new SqlConnection(cs);
conn.Open();
string sql = "SELECT * FROM Users WHERE IsActive = @active ORDER BY Id";
using var cmd = new SqlCommand(sql,conn);
cmd.Parameters.AddWithValue("@active", true);
using var reader = cmd.ExecuteReader();
Console.WriteLine("\n--- Active Users ---");
while (reader.Read())
{
    int id = reader.GetInt32(0);
    string nameA = reader.GetString(1);
    string emailA = reader.GetString(2);
    bool isActive = reader.GetBoolean(3);
    DateTime createdAt = reader.GetDateTime(4);

    Console.WriteLine($"{id}: {nameA} - {emailA} (Active={isActive}) ({createdAt})");
}
//----------DapperOrm----------
const string sqlD = "SELECT * FROM Users ORDER BY Id;";

using var connection =new SqlConnection(cs);
var users = connection.Query<User>(sqlD).ToList();

foreach (var u in users)
    Console.WriteLine($"Id: {u.Id}, Username: {u.Name}, Email: {u.Email}");

const string sqlD1 = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email);";

Console.Write("Username: ");
var name = Console.ReadLine()?.Trim();

Console.Write("Email: ");
var email = Console.ReadLine()?.Trim();
if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
{
    Console.WriteLine("Username and Email are required.");
    return;
}

var affectedRows = connection.Execute(sqlD1, new { Name = name, Email = email });

Console.WriteLine(affectedRows == 1 ? "Insert OK" : "Insert failed");
    