using Data_M1;
using System.Globalization;

class Program
{
    static void Main()
    {
        // TASK 1: Create and Manipulate Date and Time Values
        Console.WriteLine("\n--- TASK 1: Create and Manipulate Date and Time Values ---");

        // TASK 1: Step 1 - Get the current date and time, and extract date and time components
        DateTime currentDateTime = DateTime.Now;
        Console.WriteLine($"Current Date and Time: {currentDateTime}");
        Console.WriteLine($"Date Component: {currentDateTime.Date}");
        Console.WriteLine($"Time Component: {currentDateTime.TimeOfDay}");

        // TASK 1: Step 2 - Get the current day of the week and the current month and year
        Console.WriteLine($"Day of the Week: {currentDateTime.DayOfWeek}");
        Console.WriteLine($"Month and Year: {currentDateTime.ToString("MMMM yyyy")}");

        // TASK 1: Step 3 - Add days to the current date and parse a date string
        DateTime futureDate = currentDateTime.AddDays(10);
        Console.WriteLine($"Date 10 Days from Now: {futureDate}");
        string dateString = "2024-12-01";
        DateTime parsedDate = DateTime.Parse(dateString);
        Console.WriteLine($"Parsed Date: {parsedDate}");

        // TASK 1: Step 4 - Format a date and get the current timezone offset
        Console.WriteLine($"Formatted Date: {currentDateTime.ToString("yyyy-MM-dd")}");
        TimeZoneInfo localZone = TimeZoneInfo.Local;
        Console.WriteLine($"Timezone: {localZone.DisplayName}, Offset: {localZone.BaseUtcOffset}");

        // TASK 1: Step 5 - Convert the current time to UTC and display it
        DateTime utcTime = currentDateTime.ToUniversalTime();
        Console.WriteLine($"Current UTC Time: {utcTime}");

        // TASK 2: Calculate Date and Time Values for Bank Customer Transactions
        Console.WriteLine("\n--- TASK 2: Calculate Date and Time Values for Bank Customer Transactions ---");

        // New customer and accounts
        string firstName = "Tim";
        string lastName = "Shao";
        BankCustomer customer1 = new StandardCustomer(firstName, lastName);

        BankAccount account1 = new BankAccount(customer1.CustomerId, 10000);
        BankAccount account2 = new CheckingAccount(customer1.CustomerId, 500, 400);
        BankAccount account3 = new SavingsAccount(customer1.CustomerId, 1000);
        BankAccount account4 = new MoneyMarketAccount(customer1.CustomerId, 2000);

        BankAccount[] bankAccounts = new BankAccount[4];
        bankAccounts[0] = account1;
        bankAccounts[1] = account2;
        bankAccounts[2] = account3;
        bankAccounts[3] = account4;

        // TASK 2: Step 1 - Create a transaction for the current date and time
        Transaction transaction1 = new Transaction(DateOnly.FromDateTime(currentDateTime), TimeOnly.FromDateTime(currentDateTime), 100, account1.AccountNumber, account2.AccountNumber, "Transfer");
        Console.WriteLine($"Transaction: {transaction1.ReturnTransaction()}");

        // TASK 2: Step 2 - Create a transaction for yesterday at 1:15PM
        DateTime yesterday = currentDateTime.AddDays(-1).Date.AddHours(13).AddMinutes(15);
        Transaction transaction2 = new Transaction(DateOnly.FromDateTime(yesterday), TimeOnly.FromDateTime(yesterday), 200, account2.AccountNumber, account3.AccountNumber, "Deposit");
        Console.WriteLine($"Transaction: {transaction2.ReturnTransaction()}");

        // TASK 2: Step 3 - Create transactions for the first three days of December 2024
        Transaction[] decemberTransactions = new Transaction[3];
        for (int i = 0; i < 3; i++)
        {
            DateTime transactionDate = new DateTime(2024, 12, i + 1);
            decemberTransactions[i] = new Transaction(DateOnly.FromDateTime(transactionDate), TimeOnly.FromDateTime(transactionDate), 300 + i * 50, account3.AccountNumber, account4.AccountNumber, "Withdraw");
            Console.WriteLine($"Transaction: {decemberTransactions[i].ReturnTransaction()}");
        }

        // TASK 2: Step 4 - Display the datedTransactions
        Console.WriteLine("\nDisplaying all dated transactions:");
        Console.WriteLine(transaction1.ReturnTransaction());
        Console.WriteLine(transaction2.ReturnTransaction());
        foreach (var transaction in decemberTransactions)
        {
            Console.WriteLine(transaction.ReturnTransaction());
        }

        // TASK 3: Use Date Ranges to Simulate Transactions Programmatically
        Console.WriteLine("\n--- TASK 3: Use Date Ranges to Simulate Transactions Programmatically ---");

        // TASK 3: Step 1 - Define a date range starting on December 12, 2024, and ending on February 20, 2025
        DateOnly startDate = new DateOnly(2024, 12, 12);
        DateOnly endDate = new DateOnly(2025, 2, 20);

        // TASK 3: Step 2 - Generate transactions for the specified date range using the SimulateTransactions class
        Transaction[] simulatedTransactions = SimulateTransactions.SimulateTransactionsDateRange(startDate, endDate, account1, account2);

        // TASK 3: Step 3 - Display the simulated transactions
        Console.WriteLine("Summary of simulated transactions:");

        int totalDeposits = 0, totalWithdrawals = 0, totalTransfers = 0;
        double totalDepositAmount = 0, totalWithdrawalAmount = 0, totalTransferAmount = 0;

        foreach (var transaction in simulatedTransactions)
        {
            switch (transaction.TransactionType)
            {
                case "Deposit":
                    totalDeposits++;
                    totalDepositAmount += transaction.TransactionAmount;
                    break;
                case "Withdraw":
                    totalWithdrawals++;
                    totalWithdrawalAmount += transaction.TransactionAmount;
                    break;
                case "Transfer":
                    totalTransfers++;
                    totalTransferAmount += transaction.TransactionAmount;
                    break;
            }
        }

        Console.WriteLine($"Total Deposits: {totalDeposits}, Amount: {totalDepositAmount:C}");
        Console.WriteLine($"Total Withdrawals: {totalWithdrawals}, Amount: {totalWithdrawalAmount:C}");
        Console.WriteLine($"Total Transfers: {totalTransfers}, Amount: {totalTransferAmount:C}");
        Console.WriteLine($"Number of transactions processed: {simulatedTransactions.Length}");
    }
}