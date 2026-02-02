using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Files_M3;

public static class JsonStorage
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
    };

    public static void SaveBankAccount(BankAccount account, string directoryPath)
    {
        var accountDTO = new BankAccountDTO
        {
            AccountNumber = account.AccountNumber,
            CustomerId = account.CustomerId,
            Balance = account.Balance,
            AccountType = account.AccountType,
            InterestRate = account.InterestRate
        };

        string accountFilePath = Path.Combine(directoryPath, "Accounts", $"{account.AccountNumber}.json");

        var accountDirectoryPath = Path.GetDirectoryName(accountFilePath);
        if (accountDirectoryPath != null && !Directory.Exists(accountDirectoryPath))
        {
            Directory.CreateDirectory(accountDirectoryPath);
        }

        string json = JsonSerializer.Serialize(accountDTO, _options);
        File.WriteAllText(accountFilePath, json);

        SaveAllTransactions(account.Transactions, directoryPath, account.AccountNumber);
    }

    public static void SaveAllTransactions(IEnumerable<Transaction> transactions, string directoryPath, int accountNumber)
    {
        string transactionsFilePath = Path.Combine(directoryPath, "Transactions", $"{accountNumber}-transactions.json");

        var transactionsDirectoryPath = Path.GetDirectoryName(transactionsFilePath);
        if (transactionsDirectoryPath != null && !Directory.Exists(transactionsDirectoryPath))
        {
            Directory.CreateDirectory(transactionsDirectoryPath);
        }

        string json = JsonSerializer.Serialize(transactions, _options);
        File.WriteAllText(transactionsFilePath, json);
    }

    public static void SaveAllAccounts(IEnumerable<IBankAccount> accounts, string directoryPath)
    {
        foreach (var account in accounts)
        {
            SaveBankAccount((BankAccount)account, directoryPath);
        }
    }

    public static void SaveBankCustomer(BankCustomer customer, string directoryPath)
    {
        var customerDTO = new BankCustomerDTO
        {
            CustomerId = customer.CustomerId,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            AccountNumbers = new List<int>()
        };

        foreach (var account in customer.Accounts)
        {
            customerDTO.AccountNumbers.Add(account.AccountNumber);
        }

        string customerFilePath = Path.Combine(directoryPath, "Customers", $"{customer.CustomerId}.json");

        var customerDirectoryPath = Path.GetDirectoryName(customerFilePath);
        if (customerDirectoryPath != null && !Directory.Exists(customerDirectoryPath))
        {
            Directory.CreateDirectory(customerDirectoryPath);
        }

        string json = JsonSerializer.Serialize(customerDTO, _options);
        File.WriteAllText(customerFilePath, json);

        SaveAllAccounts(customer.Accounts, directoryPath);
    }

    public static void SaveAllCustomers(IEnumerable<BankCustomer> customers, string directoryPath)
    {
        foreach (var customer in customers)
        {
            SaveBankCustomer(customer, directoryPath);
        }
    }

}
