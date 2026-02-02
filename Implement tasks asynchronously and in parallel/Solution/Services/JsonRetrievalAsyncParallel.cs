using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Files_M3
{
    public static class JsonRetrievalAsyncParallel
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            MaxDepth = 64,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };

        public static async Task<BankCustomerDTO> LoadBankCustomerDTOAsync(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("No customer found.");
            }

            var customerDTO = JsonSerializer.Deserialize<BankCustomerDTO>(json, _options);
            if (customerDTO == null)
            {
                throw new Exception("Customer could not be deserialized.");
            }

            return customerDTO;
        }

        public static async Task<BankCustomer> LoadBankCustomerAsyncParallel(Bank bank, string filePath, string accountsDirectoryPath, string transactionsDirectoryPath)
        {
            var customerDTO = await LoadBankCustomerDTOAsync(filePath);

            var bankCustomer = bank.GetCustomerById(customerDTO.CustomerId);

            if (bankCustomer == null)
            {
                bankCustomer = new BankCustomer(customerDTO.FirstName, customerDTO.LastName, customerDTO.CustomerId, bank);
                bank.AddCustomer(bankCustomer);
            }

            // Process accounts in parallel
            var accountTasks = customerDTO.AccountNumbers.Select(async accountNumber =>
            {
                var existingAccount = bankCustomer.Accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);

                if (existingAccount == null)
                {
                    var accountFilePath = Path.Combine(accountsDirectoryPath, $"{accountNumber}.json");
                    var recoveredAccount = await LoadBankAccountAsync(accountFilePath, transactionsDirectoryPath, bankCustomer);

                    if (recoveredAccount != null)
                    {
                        bankCustomer.AddAccount(recoveredAccount);
                    }
                }
                else
                {
                    bankCustomer.AddAccount(existingAccount);
                }
            });

            // Wait for all accounts to be processed
            await Task.WhenAll(accountTasks);

            return bankCustomer;
        }

        public static async Task<IEnumerable<BankCustomer>> LoadAllCustomersAsyncParallel(Bank bank, string directoryPath, string accountsDirectoryPath, string transactionsDirectoryPath)
        {
            var customerFiles = Directory.GetFiles(Path.Combine(directoryPath, "Customers"), "*.json");

            // Process all customer files in parallel
            var customerTasks = customerFiles.Select(filePath =>
                LoadBankCustomerAsyncParallel(bank, filePath, accountsDirectoryPath, transactionsDirectoryPath));

            var customers = await Task.WhenAll(customerTasks);
            return customers;
        }

        public static async Task<BankAccount> LoadBankAccountAsync(string accountFilePath, string transactionsDirectoryPath, BankCustomer customer)
        {
            var accountDTO = await LoadBankAccountDTOAsync(accountFilePath);

            var existingAccount = customer.Accounts.FirstOrDefault(a => a.AccountNumber == accountDTO.AccountNumber);

            if (existingAccount != null)
            {
                return (BankAccount)existingAccount;
            }

            var recoveredBankAccount = new BankAccount(customer, customer.CustomerId, accountDTO.Balance, accountDTO.AccountType);

            string transactionsFilePath = Path.Combine(transactionsDirectoryPath, $"{accountDTO.AccountNumber}-transactions.json");

            if (File.Exists(transactionsFilePath))
            {
                var recoveredTransactions = await LoadAllTransactionsAsync(transactionsFilePath);

                // Add transactions to the account
                foreach (var transaction in recoveredTransactions)
                {
                    recoveredBankAccount.AddTransaction(transaction);
                }
            }

            return recoveredBankAccount;
        }

        public static async Task<IEnumerable<Transaction>> LoadAllTransactionsAsync(string filePath)
        {
            string jsonTransaction = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrEmpty(jsonTransaction))
            {
                throw new Exception("No transactions loaded.");
            }

            var transactions = JsonSerializer.Deserialize<IEnumerable<Transaction>>(jsonTransaction, _options);
            if (transactions == null)
            {
                throw new Exception("Transactions could not be deserialized.");
            }

            return transactions;
        }

        public static async Task<BankAccountDTO> LoadBankAccountDTOAsync(string filePath)
        {
            string json = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrEmpty(json))
            {
                throw new Exception("No account found.");
            }

            var accountDTO = JsonSerializer.Deserialize<BankAccountDTO>(json, _options);
            if (accountDTO == null)
            {
                throw new Exception("Account could not be deserialized.");
            }

            return accountDTO;
        }
    }
}