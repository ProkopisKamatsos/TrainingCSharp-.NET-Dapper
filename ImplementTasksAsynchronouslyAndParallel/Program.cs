using Files_M3;

/* 

Code that demonstrates the use of asynchronous REST API calls in C#

using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // PetStore API endpoint
                    string url = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    //Console.WriteLine($"Response: {responseBody}");

                    // Deserialize the JSON response into a list of pets
                    var pets = JsonSerializer.Deserialize<List<Pet>>(responseBody);

                    // Iterate through the list of pets and display their details
                    foreach (var pet in pets)
                    {
                        //Console.WriteLine($"Pet ID: {pet.id}, Name: {pet.name}");
                        if (pet.id.ToString().Length > 4)
                        {
                            Console.WriteLine($"Pet ID: {pet.id}, Name: {pet.name}");
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
        }
    }
}
public class Pet
{
    public long id { get; set; }
    public string name { get; set; }
    public Category category { get; set; }
    public List<string> photoUrls { get; set; }
    public List<Tag> tags { get; set; }
    public string status { get; set; }
}

public class Category
{
    public long id { get; set; }
    public string name { get; set; }
}

public class Tag
{
    public long id { get; set; }
    public string name { get; set; }
} 

*/


using System;
using System.IO;
using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Demonstrate Asynchronous tasks in C# (Clean the Solution before running this program).");

        // Create Bank objects
        Bank bank1 = new Bank(); // Bank object to load data synchronously
        Bank bank2 = new Bank(); // Bank object to load data asynchronously
        Bank bank3 = new Bank(); // Bank object to load data asynchronously and in parallel



        // Step 1: Implement async and await keywords in File storage tasks
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Console.WriteLine("\nStep 1: Implement async and await keywords in File storage tasks.");

        /*

        Create an Async version of the ApprovedCustomersLoader class: ApprovedCustomersLoaderAsync.cs
        Create an Async version of the JsonStorage class: JsonStorageAsync.cs
            - JsonSerializer.DeserializeAsync
            - File.WriteAllTextAsync

        Create an Async version of the CreateDataLogs class: CreateDataLogsAsync.cs


        */



        // Step 2: Implement File I/O tasks (load customer data files)
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        Console.WriteLine("\nStep 2: Implement File I/O tasks (load customer data files).");

        // get the time before loading the data
        DateTime timeBeforeLoadCall = DateTime.Now;

        // Load the customer data from the file
        LoadCustomerLogs.ReadCustomerData(bank1);

        // get the time after loading the data
        DateTime timeAfterLoadCall = DateTime.Now;

        // calculate the time taken to load the data
        TimeSpan timeTakenToReturn = timeAfterLoadCall - timeBeforeLoadCall;
        Console.WriteLine($"\nTime taken to return to Main: {timeTakenToReturn.TotalSeconds} seconds");

        //Console.WriteLine("\n\nPress Enter to continue...");
        //Console.ReadLine();

        // Step 3: Implement File I/O tasks asynchronously
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /*

        Create the Async version of the JsonRetrieval class: JsonRetrievalAsync.cs
        - using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.Asynchronous);
        - await JsonSerializer.DeserializeAsync<IEnumerable<Transaction>>(stream, _options);

        Create the Async version of the LoadCustomerLogs class: LoadCustomerLogsAsync.cs

        Update Main method to use async calls: 
        
            - static async Task Main()
            - var asyncLoadTask = LoadCustomerLogsAsync.ReadCustomerDataAsync(bank2);
            - await asyncLoadTask;

        */

        // wait 10 seconds before starting the async task
        //Console.WriteLine("\n\nWaiting for 10 seconds before starting the async task...");

        await Task.Delay(10000);

        // Load the customer data asynchronously from the file
        // Step 2: Load the customer data asynchronously
        Console.WriteLine("\nStep 3: Implement File I/O tasks asynchronously.");

        // Get the time before loading the data asynchronously
        DateTime timeBeforeAsyncLoadCall = DateTime.Now;

        // Start the async data loading task
        var asyncLoadTask = LoadCustomerLogsAsync.ReadCustomerDataAsync(bank2);

        DateTime timeAfterAsyncLoadCall = DateTime.Now;

        Console.WriteLine($"\nTime taken to return to Main: {(timeAfterAsyncLoadCall - timeBeforeAsyncLoadCall).TotalSeconds} seconds");

        // Wait for the async task to complete
        await asyncLoadTask;

        DateTime timeAfterAsyncLoadCompleted = DateTime.Now;

        Console.WriteLine($"Time taken to load the data asynchronously: {(timeAfterAsyncLoadCompleted - timeBeforeAsyncLoadCall).TotalSeconds} seconds");

        int countBank2Customers = 0;
        int countBank2Accounts = 0;
        int countBank2Transactions = 0;

        foreach (var customer in bank2.GetAllCustomers())
        {
            countBank2Customers++;
            foreach (var account in customer.Accounts)
            {
                countBank2Accounts++;
                foreach (var transaction in account.Transactions)
                {
                    countBank2Transactions++;
                }
            }
        }

        Console.WriteLine($"\nBank 2 information...");
        Console.WriteLine($"Number of customers: {countBank2Customers}");
        Console.WriteLine($"Number of accounts: {countBank2Accounts}");
        Console.WriteLine($"Number of transactions: {countBank2Transactions}");

        //Console.WriteLine("\n\nPress Enter to continue...");
        //Console.ReadLine();

        // Step 4: Implement File I/O tasks asynchronously and in parallel
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /*

        /*

        Create the Parallel processing version of the JsonRetrievalAsync class: JsonRetrievalAsyncParallel.cs

        Create the Parallel processing version of the LoadCustomerLogsAsync class: LoadCustomerLogsAsyncParallel.cs

        Update Main method to use async calls: 
        
            - var asyncParallelLoadTask = LoadCustomerLogsAsyncParallel.ReadCustomerDataAsyncParallel(bank3);
            - await asyncParallelLoadTask;

        */

        // wait 10 seconds before starting the parallel task
        //Console.WriteLine("\n\nWaiting for 10 seconds before starting the parallel task...");
        await Task.Delay(10000);

        Console.WriteLine("\nStep 4: Implement File I/O tasks asynchronously and in parallel.");

        // Get the time before loading the data asynchronously using parallel tasks
        DateTime timeBeforeAsyncParallelLoadCall = DateTime.Now;

        // Start the async data loading task
        var asyncParallelLoadTask = LoadCustomerLogsAsyncParallel.ReadCustomerDataAsyncParallel(bank3);

        DateTime timeAfterAsyncParallelLoadCall = DateTime.Now;

        // Wait for the async task to complete
        await asyncParallelLoadTask;

        DateTime timeAfterAsyncParallelLoadCompleted = DateTime.Now;

        Console.WriteLine($"\nTime taken to return to Main: {(timeAfterAsyncParallelLoadCall - timeBeforeAsyncParallelLoadCall).TotalSeconds} seconds");
        Console.WriteLine($"Time taken to load the data asynchronously and in parallel: {(timeAfterAsyncParallelLoadCompleted - timeBeforeAsyncParallelLoadCall).TotalSeconds} seconds");

        int countBank3Customers = 0;
        int countBank3Accounts = 0;
        int countBank3Transactions = 0;

        foreach (var customer in bank3.GetAllCustomers())
        {
            countBank3Customers++;
            foreach (var account in customer.Accounts)
            {
                countBank3Accounts++;
                foreach (var transaction in account.Transactions)
                {
                    countBank3Transactions++;
                }
            }
        }

        Console.WriteLine($"\nBank 3 information...");
        Console.WriteLine($"Number of customers: {countBank3Customers}");
        Console.WriteLine($"Number of accounts: {countBank3Accounts}");
        Console.WriteLine($"Number of transactions: {countBank3Transactions}");

        Console.WriteLine("\n\nPress Enter to exit...");
        Console.ReadLine();
    }
}
