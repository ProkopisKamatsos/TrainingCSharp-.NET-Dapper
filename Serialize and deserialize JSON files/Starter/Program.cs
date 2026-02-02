using Files_M2;
using System.Text.Json;

using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("Demonstrate JSON file storage and retrieval using BankCustomer, BankAccount, and Transaction classes");

        // Create a Bank object
        Bank bank = new Bank();

        // Create a bank customer named Niki Demetriou
        string firstName = "Niki";
        string lastName = "Demetriou";
        BankCustomer bankCustomer = new BankCustomer(firstName, lastName);

        // Add Checking, Savings, and MoneyMarket accounts to bankCustomer
        bankCustomer.AddAccount(new CheckingAccount(bankCustomer, bankCustomer.CustomerId, 5000));
        bankCustomer.AddAccount(new SavingsAccount(bankCustomer, bankCustomer.CustomerId, 15000));
        bankCustomer.AddAccount(new MoneyMarketAccount(bankCustomer, bankCustomer.CustomerId, 90000));

        // Add the bank customer to the bank object
        bank.AddCustomer(bankCustomer);

        // Simulate one month of transactions for customer Niki Demetriou
        DateOnly startDate = new DateOnly(2025, 2, 1);
        DateOnly endDate = new DateOnly(2025, 2, 28);
        bankCustomer = SimulateDepositsWithdrawalsTransfers.SimulateActivityDateRange(startDate, endDate, bankCustomer);

        // Get the current directory of the executable program
        string currentDirectory = Directory.GetCurrentDirectory();

        // Use currentDirectory to create a directory path named BankLogs
        string bankLogsDirectoryPath = Path.Combine(currentDirectory, "BankLogs");
        if (!Directory.Exists(bankLogsDirectoryPath))
        {
            Directory.CreateDirectory(bankLogsDirectoryPath);
            //Console.WriteLine($"Created directory: {bankLogsDirectoryPath}");
        }

        // Get the first transaction from the first account of the bank customer
        Transaction singleTransaction = bankCustomer.Accounts[0].Transactions.ElementAt(0);

        // Serialize the transaction object using JsonSerializer.Serialize
        string transactionJson = JsonSerializer.Serialize(singleTransaction);

        // Display the JSON string
        Console.WriteLine($"\nJSON string: {transactionJson}");

        // Convert the JSON string into a Transaction objects using JsonSerializer.Deserialize
        Transaction? deserializedTransaction = JsonSerializer.Deserialize<Transaction>(transactionJson);

        if (deserializedTransaction == null)
        {
            Console.WriteLine("Deserialization failed. Check the Transaction class for public setters and a parameterless constructor.");
        }
        else
        {
            // Use the Transaction.ReturnTransaction method to display transaction information
            Console.WriteLine($"\nDeserialized transaction object: {deserializedTransaction.ReturnTransaction()}");
        }
        // Serialize account transactions using JsonSerializer.Serialize
        string transactionsJson = JsonSerializer.Serialize(bankCustomer.Accounts[0].Transactions);
        Console.WriteLine($"\nbankCustomer.Accounts[0].Transactions serialized to JSON: \n{transactionsJson}");

        // Construct a file path where the serialized transactions (JSON string) can be stored
        string transactionsJsonFilePath = Path.Combine(bankLogsDirectoryPath, "Transactions", bankCustomer.Accounts[0].AccountNumber.ToString() + "-transactions" + ".json");

        // Create the parent directory for the serialized transactions file
        var directoryPath = Path.GetDirectoryName(transactionsJsonFilePath);
        if (directoryPath != null && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Store the serialized JSON string to a file
        File.WriteAllText(transactionsJsonFilePath, transactionsJson);
        Console.WriteLine($"\nSerialized transactions saved to: {transactionsJsonFilePath}");

        // Use File.ReadAllText to read the JSON file and assign the text contents to a string.
        string transactionsJsonFromFile = File.ReadAllText(transactionsJsonFilePath);

        // Deserialize the JSON string using JsonSerializer.Deserialize
        var transactionsJsonDeserialized = JsonSerializer.Deserialize<IEnumerable<Transaction>>(transactionsJsonFromFile);

        // Loop through the deserialized transactions and display each transaction
        if (transactionsJsonDeserialized == null)
        {
            Console.WriteLine("Deserialization failed. Check the Transaction class for public setters and a parameterless constructor.");
        }
        else
        {
            Console.WriteLine("\nDeserialized transactions:");
            foreach (var transaction in transactionsJsonDeserialized)
            {
                Console.WriteLine(transaction.ReturnTransaction());
            }
        }
        // Configure JsonSerializerOptions
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve // Handle circular references
        };

        //Serialize the CheckingAccount object using JsonSerializer.Serialize
        string accountJson = JsonSerializer.Serialize(bankCustomer.Accounts[0], options);

        Console.WriteLine(accountJson);

        // Create a file path for the CheckingAccount object
        string accountFilePath = Path.Combine(bankLogsDirectoryPath, "Account", bankCustomer.Accounts[0].AccountNumber + ".json");

        // Create the parent directory for the serialized account file
        var accountDirectoryPath = Path.GetDirectoryName(accountFilePath);
        if (accountDirectoryPath != null && !Directory.Exists(accountDirectoryPath))
        {
            Directory.CreateDirectory(accountDirectoryPath);
        }

        // Save the JSON to a file
        File.WriteAllText(accountFilePath, accountJson);
        Console.WriteLine($"Serialized account saved to: {accountFilePath}");
        string accountJsonFromFile = File.ReadAllText(accountFilePath);

        // Deserialize the JSON string using JsonSerializer.Deserialize with options
        try
        {
            BankAccount? deserializedAccount = JsonSerializer.Deserialize<BankAccount>(accountJsonFromFile, options);

            // Demonstrate the deserialized BankAccount object
            if (deserializedAccount == null)
            {
                Console.WriteLine("Deserialization failed. Check the BankAccount class for public setters and a parameterless constructor.");
            }
            else
            {
                Console.WriteLine($"\nDeserialized BankAccount object: {deserializedAccount.DisplayAccountInfo()}");
                Console.WriteLine($"Transactions for Account Number: {deserializedAccount.AccountNumber}");

                foreach (var transaction in deserializedAccount.Transactions)
                {
                    Console.WriteLine(transaction.ReturnTransaction());
                }
            }
        }
        catch (Exception ex)
        {
            string displayMessage = "Exception has occurred: " + ex.Message.Split('.')[0] + ".";
            displayMessage += "\n\nConsider using Data Transfer Objects (DTOs) for serializing and deserializing complex and nested objects.";
            Console.WriteLine(displayMessage);
        }

    }

}