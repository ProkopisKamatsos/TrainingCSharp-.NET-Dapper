using Microsoft.Data.SqlClient;

const string cs =
    "Server=localhost\\SQLEXPRESS;Database=AdoNetPractice;Trusted_Connection=True;TrustServerCertificate=True;";

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== ADO.NET CRUD Practice ===");
    Console.WriteLine("1) SELECT active users");
    Console.WriteLine("2) INSERT user");
    Console.WriteLine("3) UPDATE user name by Id");
    Console.WriteLine("4) DELETE user by Id (hard delete)");
    Console.WriteLine("0) Exit");
    Console.Write("Choose: ");

    var choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                SelectActiveUsers();
                break;
            case "2":
                InsertUser();
                break;
            case "3":
                UpdateUserNameById();
                break;
            case "4":
                DeleteUserById();
                break;
            case "0":
                return;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
    catch (SqlException ex)
    {
        Console.WriteLine("SQL Error: " + ex.Message);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

void SelectActiveUsers()
{
    using var conn = new SqlConnection(cs);
    conn.Open();

    using var cmd = new SqlCommand(
        "SELECT Id, Name, Email, IsActive, CreatedAt " +
        "FROM Users WHERE IsActive = @active ORDER BY Id",
        conn
    );

    cmd.Parameters.AddWithValue("@active", true);

    using var reader = cmd.ExecuteReader();

    Console.WriteLine("\n--- Active Users ---");
    while (reader.Read())
    {
        int id = reader.GetInt32(0);
        string name = reader.GetString(1);
        string email = reader.GetString(2);
        bool isActive = reader.GetBoolean(3);
        DateTime createdAt = reader.GetDateTime(4);

        Console.WriteLine($"{id}: {name} - {email} (Active={isActive}) ({createdAt})");
    }
}

void InsertUser()
{
    Console.Write("Name: ");
    var name = Console.ReadLine()?.Trim();

    Console.Write("Email: ");
    var email = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Name and Email are required.");
        return;
    }

    using var conn = new SqlConnection(cs);
    conn.Open();

    // INSERT (parameterized)
    using var cmd = new SqlCommand(
        "INSERT INTO Users (Name, Email, IsActive) VALUES (@name, @email, @active)",
        conn
    );

    cmd.Parameters.AddWithValue("@name", name);
    cmd.Parameters.AddWithValue("@email", email);
    cmd.Parameters.AddWithValue("@active", true);

    int rows = cmd.ExecuteNonQuery();
    Console.WriteLine($"Inserted rows: {rows}");
}

void UpdateUserNameById()
{
    Console.Write("User Id to update: ");
    var idText = Console.ReadLine();

    if (!int.TryParse(idText, out int id))
    {
        Console.WriteLine("Invalid Id.");
        return;
    }

    Console.Write("New Name: ");
    var newName = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(newName))
    {
        Console.WriteLine("New Name is required.");
        return;
    }

    using var conn = new SqlConnection(cs);
    conn.Open();

    using var cmd = new SqlCommand(
        "UPDATE Users SET Name = @name WHERE Id = @id",
        conn
    );

    cmd.Parameters.AddWithValue("@name", newName);
    cmd.Parameters.AddWithValue("@id", id);

    int rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows == 0 ? "No rows updated (Id not found)." : $"Updated rows: {rows}");
}

void DeleteUserById()
{
    Console.Write("User Id to delete: ");
    var idText = Console.ReadLine();

    if (!int.TryParse(idText, out int id))
    {
        Console.WriteLine("Invalid Id.");
        return;
    }

    using var conn = new SqlConnection(cs);
    conn.Open();

    using var cmd = new SqlCommand(
        "DELETE FROM Users WHERE Id = @id",
        conn
    );

    cmd.Parameters.AddWithValue("@id", id);

    int rows = cmd.ExecuteNonQuery();
    Console.WriteLine(rows == 0 ? "No rows deleted (Id not found)." : $"Deleted rows: {rows}");
}
