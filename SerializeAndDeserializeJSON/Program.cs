using Files_M2;

using System;
using System.IO;
using System.Text;
using System.Text.Json;

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

        // Create directory paths for Account and Transaction files
        string accountsDirectoryPath = Path.Combine(bankLogsDirectoryPath, "Accounts");
        Directory.CreateDirectory(accountsDirectoryPath);

        string transactionsDirectory = Path.Combine(bankLogsDirectoryPath, "Transactions");
        Directory.CreateDirectory(transactionsDirectory);

        BankAccount customerAccount1 = (BankAccount)bankCustomer.Accounts[0];

        // Create a jsonAccountDTOFilePath for the BankAccount object
        string jsonAccountDTOFilePath = Path.Combine(accountsDirectoryPath, customerAccount1.AccountNumber.ToString() + ".json");

        // Create a bankAccountDTO object from the BankAccount object and serialize it as JSON
        BankAccountDTO bankAccountDTO = BankAccountDTO.MapToDTO((BankAccount)customerAccount1);
        string jsonAccountDTO = JsonSerializer.Serialize(bankAccountDTO, options);

        // Save the serialized jsonAccountDTO to a file in the Accounts directory using the AccountNumber value as the filename  (.json)
        File.WriteAllText(jsonAccountDTOFilePath, jsonAccountDTO);
        Console.WriteLine($"\nSerialized account saved to: {jsonAccountDTOFilePath}");

        //Serialize the Transactions collection
        string jsonTransactions = JsonSerializer.Serialize(customerAccount1.Transactions);

        // Create a jsonTransactionsFilePath for the Transactions collection using the account number and a "T" suffix as the filename  (.json)
        string jsonTransactionsFilePath = Path.Combine(transactionsDirectory, customerAccount1.AccountNumber.ToString() + "T" + ".json");

        // Save the serialized transaction to a file in the Transactions directory 
        File.WriteAllText(jsonTransactionsFilePath, jsonTransactions);
        Console.WriteLine($"Serialized account transactions saved to: {jsonTransactionsFilePath}");

        // Load the BankAccountDTO info from the JSON file
        jsonAccountDTO = File.ReadAllText(jsonAccountDTOFilePath);

        // Deserialize the JSON string into a BankAccountDTO object using JsonSerializer.Deserialize
        var accountDTO = JsonSerializer.Deserialize<BankAccountDTO>(jsonAccountDTO, options);

        if (accountDTO == null)
        {
            Console.WriteLine("Deserialization failed. Check the BankAccountDTO class for public setters and a parameterless constructor.");
        }
        else
        {
            // create a bank account using the recovered data
            var recoveredBankAccount = new BankAccount(bankCustomer, bankCustomer.CustomerId, accountDTO.Balance, accountDTO.AccountType);

            // load the transactions file into a JSON formatted string
            jsonTransactions = File.ReadAllText(jsonTransactionsFilePath);

            // deserialize the JSON string into a collection of Transaction objects
            var transactions = JsonSerializer.Deserialize<IEnumerable<Transaction>>(jsonTransactions, options);

            if (transactions == null)
            {
                Console.WriteLine("Deserialization failed. Check the Transaction class for public setters and a parameterless constructor.");
            }
            else
            {
                // add the transactions to the recovered account
                foreach (var transaction in transactions)
                {
                    recoveredBankAccount.AddTransaction(transaction);
                }

                // display the recovered account information
                Console.WriteLine($"\nRecovered BankAccount object: {recoveredBankAccount.DisplayAccountInfo()}");
                Console.WriteLine($"Transactions for Account Number: {recoveredBankAccount.AccountNumber}\n");

                foreach (var transaction in recoveredBankAccount.Transactions)
                {
                    Console.WriteLine(transaction.ReturnTransaction());
                }
            }
        }





        // Get the customer's checking account
        BankAccount checkingAccount = (CheckingAccount)bankCustomer.Accounts[0];

        // Use the JsonStorage.SaveBankAccount method to save account info to JSON files (an account file using BankAccountDTO and a separate transactions file)
        JsonStorage.SaveBankAccount(checkingAccount, bankLogsDirectoryPath);

        // Construct the file path for the checking account
        string retrieveAccountFilePath = JsonRetrieval.ReturnAccountFilePath(bankLogsDirectoryPath, checkingAccount.AccountNumber);

        // Use the JsonStorage.LoadBankAccount method to load account info from JSON files (an account file using BankAccountDTO and a separate transactions file)
        BankAccount retrievedAccount = JsonRetrieval.LoadBankAccount(retrieveAccountFilePath, transactionsDirectory, bankCustomer);

        // Display the retrieved account information
        Console.WriteLine($"The owner of the retrieved account is: {retrievedAccount.Owner.ReturnFullName()}");
        Console.WriteLine($"{retrievedAccount.Owner.ReturnFullName()} has the following {retrievedAccount.Owner.Accounts.Count} accounts:");
        foreach (var account in retrievedAccount.Owner.Accounts)
        {
            Console.WriteLine($"Account number: {account.AccountNumber} is a {account.AccountType} account.");
        }

        Console.WriteLine($"\nRetrieved {retrievedAccount.AccountType} account info: {retrievedAccount.DisplayAccountInfo()}");

        Console.WriteLine($"The following transactions were retrieved for {retrievedAccount.Owner.ReturnFullName()}'s {retrievedAccount.AccountType} account: \n");

        foreach (var transaction in retrievedAccount.Transactions)
        {
            Console.WriteLine(transaction.ReturnTransaction());
        }

        string customersDirectoryPath = Path.Combine(bankLogsDirectoryPath, "Customers");
        Directory.CreateDirectory(customersDirectoryPath);

        JsonStorage.SaveBankCustomer(bankCustomer, bankLogsDirectoryPath);
        Console.WriteLine($"\nBank customer information for {bankCustomer.ReturnFullName()} backed up to JSON files.");

        // Delete the customer and then start the recovery process
        bank.RemoveCustomer(bankCustomer);

        string customerFilePath = Path.Combine(customersDirectoryPath, bankCustomer.CustomerId + ".json");

        BankCustomer retrievedCustomer = JsonRetrieval.LoadBankCustomer(bank, customerFilePath, accountsDirectoryPath, transactionsDirectory);

        Console.WriteLine($"\nRetrieved customer information for {retrievedCustomer.ReturnFullName()}:");
        Console.WriteLine($"Customer ID: {retrievedCustomer.CustomerId}");
        Console.WriteLine($"First Name: {retrievedCustomer.FirstName}");
        Console.WriteLine($"Last Name: {retrievedCustomer.LastName}");
        Console.WriteLine($"Number of accounts: {retrievedCustomer.Accounts.Count}");

        foreach (var account in retrievedCustomer.Accounts)
        {
            Console.WriteLine($"\nAccount number: {account.AccountNumber} is a {account.AccountType} account.");
            Console.WriteLine($" - Balance: {account.Balance}");
            Console.WriteLine($" - Interest Rate: {account.InterestRate}");
            Console.WriteLine($" - Transactions:");
            foreach (var transaction in account.Transactions)
            {
                Console.WriteLine($"    {transaction.ReturnTransaction()}");
            }
        }
    }
}