using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Employee
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
}

class Program
{
    static void Main()
    {

        Console.WriteLine("--------------deserialization---------------");
        string jsonString = "{\"Name\":\"Anette Thomsen\",\"Age\":30,\"Address\":\"123 Main St\"}";
        string jsonStringDE = @"{""Name"":""Anette Thomsen"",""Age"":30,""Address"":""123 Main St""}";
        var customerDE = JsonSerializer.Deserialize<Employee>(jsonStringDE);
        Console.WriteLine($"Name: {customerDE.Name}, Age: {customerDE.Age}, Address: {customerDE.Address}");


        Console.WriteLine("--------------serialization----------------");
        var customerSE = new Employee { Name = "Anette Thomsen", Age = 30, Address = "123 Main St" };
        string jsonStringSE = JsonSerializer.Serialize(customerSE);
        Console.WriteLine(jsonStringSE);

    }
}


