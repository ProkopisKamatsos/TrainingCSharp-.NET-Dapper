using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using DapperTrain;

public class Program
{
    private const string ConnectionString =
        "Server=localhost\\SQLEXPRESS;Database=DapperTrain;Trusted_Connection=True;TrustServerCertificate=True;";

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== MINI CRUD (Users) ===");
            Console.WriteLine("1. List users");
            Console.WriteLine("2. Add user");
            Console.WriteLine("3. Update user");
            Console.WriteLine("4. Delete user");
            Console.WriteLine("5. Find user by Id");
            Console.WriteLine("6. Exit");

            Console.Write("Choose: ");

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    ListUsers();
                    break;
                case "2":
                    AddUser();
                    break;
                case "3":
                    UpdateUser();
                    break;
                case "4":
                    DeleteUser();
                    break;
                case "5":
                    FindUserById();
                    break;
                case "6":
                    Console.WriteLine("Bye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    private static SqlConnection CreateConnection()
        => new SqlConnection(ConnectionString);

    private static void ListUsers()
    {
        const string sql = "SELECT Id, Username, Email FROM Users ORDER BY Id;";

        using var connection = CreateConnection();
        var users = connection.Query<User>(sql).ToList();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var u in users)
            Console.WriteLine($"Id: {u.Id}, Username: {u.Username}, Email: {u.Email}");
    }

    private static void AddUser()

    {

        const string sql = "INSERT INTO Users (Username, Email) VALUES (@Username, @Email);";

        Console.Write("Username: ");
        var username = Console.ReadLine()?.Trim();

        Console.Write("Email: ");
        var email = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Username and Email are required.");
            return;
        }


        using var connection = CreateConnection();
        var affectedRows = connection.Execute(sql, new { Username = username, Email = email });

        Console.WriteLine(affectedRows == 1 ? "Insert OK" : "Insert failed");
    }

    private static void UpdateUser()
    {
        const string sql = "UPDATE Users SET Username = @Username, Email = @Email WHERE Id = @Id";


        ListUsers();

        Console.Write("Id to update: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int id))
        {
            Console.WriteLine("Invalid Id");
            return;
        }

        Console.Write("New Username: ");
        var username = Console.ReadLine()?.Trim();
        Console.Write("New Email:");
        var email = Console.ReadLine()?.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
        {
            Console.WriteLine("Username and Email are required.");
            return;
        }

        using var connection = CreateConnection();
        var affectedRows = connection.Execute(sql, new { Id = id, Username = username, Email = email });

        Console.WriteLine(affectedRows == 1 ? "Update OK" : "User not found");
    }

    private static void DeleteUser()
    {
        const string sql = "DELETE FROM Users WHERE Id = @Id;";

        ListUsers();

        Console.Write("Id to delete: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int id))
        {
            Console.WriteLine("Invalid Id");
            return;
        }

        using var connection = CreateConnection();
        var affectedRows = connection.Execute(sql, new { Id = id });

        Console.WriteLine(affectedRows == 1 ? "Delete OK" : "User not found");
    }
    private static void FindUserById()
    {
        ListUsers();
        const string sql = "SELECT Id, Username, Email FROM Users WHERE Id = @Id;";
        Console.Write("Id to find: ");
        if (!int.TryParse(Console.ReadLine()?.Trim(), out int id))
        {
            Console.WriteLine("Invalid Id");
            return;
        }
        using var connection = CreateConnection();
        var user = connection.QueryFirstOrDefault<User>(sql, new { Id = id });
        if (user == null)
        {
            Console.WriteLine("User not found.");
            return;
        }
        Console.WriteLine($"Id: {user.Id}, Username: {user.Username}, Email: {user.Email}");
    }
}


