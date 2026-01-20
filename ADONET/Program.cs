using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        var cs =
            @"Data Source=localhost\SQLEXPRESS;
              Initial Catalog=MiniShopDB;
              Integrated Security=True;
              Encrypt=True;
              TrustServerCertificate=True;";

        try
        {
            using var conn = new SqlConnection(cs);
            conn.Open();

            // 1) Η SQL εντολή που θέλουμε να τρέξει στη βάση
            var sql = "SELECT TOP (10) Id, Name, Price FROM dbo.Products ORDER BY Id;";

            // 2) Το SqlCommand "κουβαλάει" τη SQL προς τη βάση
            using var cmd = new SqlCommand(sql, conn);

            // 3) Το ExecuteReader γυρίζει SqlDataReader για να διαβάσουμε γραμμή-γραμμή
            using var reader = cmd.ExecuteReader();

            Console.WriteLine("---- PRODUCTS ----");

            // 4) Read() = πάμε στην επόμενη γραμμή αποτελεσμάτων
            while (reader.Read())
            {
                // Παίρνουμε τις στήλες με βάση τη σειρά που τις ζητήσαμε στο SELECT
                int id = reader.GetInt32(0);       // Id
                string name = reader.GetString(1); // Name
                decimal price = reader.GetDecimal(2); // Price

                Console.WriteLine($"{id} | {name} | {price}");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("❌ SQL Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ General Error: " + ex.Message);
        }
    }
}
