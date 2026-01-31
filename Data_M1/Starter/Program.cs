using Data_M1;
using System.Globalization;

class Program
{
    static void Main()
    {
        // Prepare banking objects 
        string firstName = "Tim";
        string lastName = "Shao";
        BankCustomer customer1 = new StandardCustomer(firstName, lastName);

        BankAccount account1 = new BankAccount(customer1.CustomerId, 10000);
        BankAccount account2 = new CheckingAccount(customer1.CustomerId, 500, 400);
        BankAccount account3 = new SavingsAccount(customer1.CustomerId, 1000);

        BankAccount[] bankAccounts = new BankAccount[4];
        bankAccounts[0] = account1;
        bankAccounts[1] = account2;
        bankAccounts[2] = account3;

        Transaction[] datedTransactions = new Transaction[5];

        // Begin date and time operations
        Console.WriteLine("\nDemonstrate date and time operations:");

        // TASK 1: Create and Manipulate Date and Time Values
        // TASK 1: Step 1 - Get the current date and time, and extract date and time components
        DateTime currentDateTime = DateTime.Now;
        Console.WriteLine($"Current Date and Time: {currentDateTime}");

        // TASK 1: Step 2 - Get the current day of the week and the current month and year
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        Console.WriteLine($"Current Date: {currentDate}");
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
        Console.WriteLine($"Current Time: {currentTime}");
        DayOfWeek currentDayOfWeek = DateTime.Now.DayOfWeek;
        Console.WriteLine($"Current Day of the Week: {currentDayOfWeek}");
        int currentMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;
        Console.WriteLine($"Current Month: {currentMonth}, Current Year: {currentYear}");

        // TASK 1: Step 3 - Add days to the current date and parse a date string
        DateTime datePlusDays = DateTime.Now.AddDays(10);
        Console.WriteLine($"Date Plus 10 Days: {datePlusDays}");
        DateTime parsedDate = DateTime.Parse("2025-03-13");
        Console.WriteLine($"Parsed Date: {parsedDate}");
        // TASK 1: Step 4 - Format a date and get the current timezone offset
        string formattedDate = DateTime.Now.ToString("yyyy-MM-dd");
        Console.WriteLine($"Formatted Date: {formattedDate}");
        TimeZoneInfo currentTimeZone = TimeZoneInfo.Local;
        TimeSpan offsetFromUtc = currentTimeZone.GetUtcOffset(DateTime.Now);
        Console.WriteLine($"Current Time Zone: {currentTimeZone.DisplayName}, Offset from UTC: {offsetFromUtc}");
        // TASK 1: Step 5 - Convert the current time to UTC and display it
        DateTime utcTime = DateTime.UtcNow;
        Console.WriteLine($"UTC Time: {utcTime}");

        // TASK 2: Calculate Date and Time Values for Bank Customer Transactions
        // This task will display the output for customer transactions.




        // TASK 2: Step 1 - Create a transaction for the current date and time
        datedTransactions[0] = new Transaction(currentDate, currentTime, 100, account2.AccountNumber, account2.AccountNumber, "Withdraw", "Groceries");

        // TASK 2: Step 2 - Create a transaction for yesterday at 1:15PM


        datedTransactions[1] = new Transaction(currentDate.AddDays(-1), new TimeOnly(13, 15), 500, account2.AccountNumber, account2.AccountNumber, "Deposit", "ATM Deposit");

        // TASK 2: Step 3 - Create transactions for the first three days of December 2025

        datedTransactions[2] = new Transaction(new DateOnly(2025, 12, 1), new TimeOnly(10, 0), 200, account2.AccountNumber, account2.AccountNumber, "Deposit", "Salary");
        datedTransactions[3] = new Transaction(new DateOnly(2025, 12, 2), new TimeOnly(14, 30), 150, account2.AccountNumber, account2.AccountNumber, "Withdraw", "Groceries");
        datedTransactions[4] = new Transaction(new DateOnly(2025, 12, 3), new TimeOnly(9, 45), 300, account2.AccountNumber, account2.AccountNumber, "Deposit", "Freelance Work");


        // TASK 2: Step 4 - Display the datedTransactions

        Console.WriteLine("\nDated Transactions:");
        foreach (Transaction transaction in datedTransactions)
        {
            Console.WriteLine(transaction.ReturnTransaction());
        }

        // TASK 3: Use Date Ranges to Simulate Transactions Programmatically
        // TASK 3: Step 1 - Define a date range starting on October 12, 2025, and ending on December 20, 2025
        DateOnly startDate = new DateOnly(2025, 10, 12);
        DateOnly endDate = new DateOnly(2025, 12, 20);

        // TASK 3: Step 2 - Generate transactions for the specified date range using the SimulateTransactions class
        List<Transaction> transactions = new List<Transaction>(SimulateTransactions.SimulateTransactionsDateRange(startDate, endDate, account2, account3));

        // TASK 3: Step 3 - Display the simulated transactions
        Console.WriteLine("\nSimulated Transactions:");
        foreach (Transaction transaction in transactions)
        {
            if (transaction != null)
            {
                Console.WriteLine(transaction.ReturnTransaction());
            }
        }

        // TASK 3: Step 4 - Display the number of transactions processed

        Console.WriteLine($"\nNumber of transactions processed: {transactions.Count}");
    }
}