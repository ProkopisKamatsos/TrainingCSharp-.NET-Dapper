public delegate int CompareCustomers(Customer c1, Customer c2);
public class TestClass
{
    static void Main()
    {
        Random random = new Random();
        List<Customer> customers = new List<Customer>();

        // Create 10 customers with random ages and IDs
        for (int i = 0; i < 10; i++)
        {
            string customerId = random.Next(10000000, 99999999).ToString();
            int age = random.Next(30, 51); // Random age between 30 and 50
            customers.Add(new Customer(customerId, age));
        }

        // Define the CompareCustomers delegate
        CompareCustomers compare = (c1, c2) => c2.Age.CompareTo(c1.Age);
        customers.Sort((c1, c2) => compare(c1, c2));

        // Print sorted customers
        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.CustomerId}, Age: {customer.Age}");
        }
    }
}
public class Customer
{
    public string CustomerId { get; set; }
    public int Age { get; set; }

    public Customer(string custId, int age)
    {
        CustomerId = custId;
        Age = age;
    }
}
