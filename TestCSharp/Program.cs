using Dapper;
using Microsoft.Data.SqlClient;
using TestCSharp;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString =
            "Server=localhost\\SQLEXPRESS;Database=DapperTrain;Trusted_Connection=True;TrustServerCertificate=True;";

        using (var connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT * FROM Users WHERE Id = @Id";

            Console.WriteLine("Type the Id:");

            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid Id.");
                return;
            }

            var user = connection.QueryFirst<User>(sql, new { Id = id });

            if (user != null)
            {
                Console.WriteLine(user.Username);
            }
            else
            {
                Console.WriteLine("User not found");
            }
        }
    }
}
