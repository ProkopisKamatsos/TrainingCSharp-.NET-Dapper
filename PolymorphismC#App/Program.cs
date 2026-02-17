using Reuse_M2;
using System.Globalization;

class Program
{
    static void Main()
    {
        // // Demonstrate polymorphism using interfaces
        // Console.WriteLine("Demonstrating polymorphism by treating class types as instances of the same interface...");

        // string firstName = "Sandy";
        // string lastName = "Zoeng";

        // // Create an instance of IBankCustomer using BankCustomer
        // IBankCustomer customer1 = new BankCustomer(firstName, lastName);

        // // Create instances of IBankAccount that reference base and derived class types
        // Console.WriteLine("1. Create instances of IBankAccount that reference base and derived class types");
        // IBankAccount account1 = new BankAccount(customer1.CustomerId, 10000);
        // IBankAccount account2 = new CheckingAccount(customer1.CustomerId, 500, 400);
        // IBankAccount account3 = new SavingsAccount(customer1.CustomerId, 1000);

        // // Demonstrate polymorphism by treating different types of accounts as instances of the same interface.
        // Console.WriteLine("2. Demonstrate polymorphism by treating different types of accounts as instances of the same interface");
        // Console.WriteLine(account1.DisplayAccountInfo());
        // Console.WriteLine(account2.DisplayAccountInfo());
        // Console.WriteLine(account3.DisplayAccountInfo());

        // // Demonstrate interface-based polymorphism by creating instances of AccountReportGenerator and CustomerReportGenerator
        // Console.WriteLine("\nDemonstrating interface-based polymorphism...");

        // // Create instances of IReportGenerator using interfaces
        // Console.WriteLine("1. Create instances of IMonthlyReportGenerator, IQuarterlyReportGenerator, and IYearlyReportGenerator that reference CustomerReportGenerator and AccountReportGenerator");
        // IMonthlyReportGenerator customerMonthlyReportGenerator = new CustomerReportGenerator(customer1);
        // IQuarterlyReportGenerator customerQuarterlyReportGenerator = new CustomerReportGenerator(customer1);
        // IYearlyReportGenerator customerYearlyReportGenerator = new CustomerReportGenerator(customer1);

        // IMonthlyReportGenerator accountMonthlyReportGenerator = new AccountReportGenerator(account1);
        // IQuarterlyReportGenerator accountQuarterlyReportGenerator = new AccountReportGenerator(account1);
        // IYearlyReportGenerator accountYearlyReportGenerator = new AccountReportGenerator(account1);

        // Console.WriteLine("2. Demonstrate polymorphism by showing that different report generators can be used interchangeably through their interfaces.");
        // customerMonthlyReportGenerator.GenerateMonthlyReport();
        // customerMonthlyReportGenerator.GenerateCurrentMonthToDateReport();
        // customerMonthlyReportGenerator.GeneratePrevious30DayReport();
        // customerQuarterlyReportGenerator.GenerateQuarterlyReport();
        // customerYearlyReportGenerator.GeneratePreviousYearReport();
        // customerYearlyReportGenerator.GenerateCurrentYearToDateReport();
        // customerYearlyReportGenerator.GenerateLast365DaysReport();

        // accountMonthlyReportGenerator.GenerateMonthlyReport();
        // accountMonthlyReportGenerator.GenerateCurrentMonthToDateReport();
        // accountMonthlyReportGenerator.GeneratePrevious30DayReport();
        // accountQuarterlyReportGenerator.GenerateQuarterlyReport();
        // accountYearlyReportGenerator.GeneratePreviousYearReport();
        // accountYearlyReportGenerator.GenerateCurrentYearToDateReport();
        // accountYearlyReportGenerator.GenerateLast365DaysReport();

        // Demonstrate inheritance-based polymorphism
        Console.WriteLine("\nDemonstrating inheritance-based polymorphism...");
        // Create a new customer and accounts for inheritance-based polymorphism
        Console.WriteLine("Creating BankCustomer and BankAccount objects...");
        string firstName = "Tim";
        string lastName = "Shao";
        BankCustomer customer2 = new BankCustomer(firstName, lastName);

        // Create accounts for customer1
        Console.WriteLine("Creating BankAccount objects for customer1...");
        BankAccount bankAccount1 = new BankAccount(customer2.CustomerId, 10000);
        BankAccount bankAccount2 = new CheckingAccount(customer2.CustomerId, 500, 400);
        BankAccount bankAccount3 = new SavingsAccount(customer2.CustomerId, 1000);
        BankAccount bankAccount4 = new MoneyMarketAccount(customer2.CustomerId, 2000);

        BankAccount[] bankAccounts = new BankAccount[4];

        bankAccounts[0] = bankAccount1;
        bankAccounts[1] = bankAccount2;
        bankAccounts[2] = bankAccount3;
        bankAccounts[3] = bankAccount4;

        // Demonstrate polymorphism by accessing overridden methods from the base class reference
        Console.WriteLine("\nDemonstrating polymorphism by accessing overridden methods from the base class reference:");

        foreach (BankAccount account in bankAccounts)
        {
            Console.WriteLine(account.DisplayAccountInfo());
        }

        foreach (BankAccount account in bankAccounts)
        {
            if (account is CheckingAccount checkingAccount)
            {
                // CheckingAccount: Withdraw within overdraft limit
                Console.WriteLine("\nCheckingAccount: Attempting to withdraw $600 (within overdraft limit)...");
                checkingAccount.Withdraw(600);
                Console.WriteLine(checkingAccount.DisplayAccountInfo());

                // CheckingAccount: Withdraw exceeding overdraft limit
                Console.WriteLine("\nCheckingAccount: Attempting to withdraw $1000 (exceeding overdraft limit)...");
                checkingAccount.Withdraw(1000);
                Console.WriteLine(checkingAccount.DisplayAccountInfo());
            }
            else if (account is SavingsAccount savingsAccount)
            {
                // SavingsAccount: Withdraw within limit
                Console.WriteLine("\nSavingsAccount: Attempting to withdraw $200 (within withdrawal limit)...");
                savingsAccount.Withdraw(200);
                Console.WriteLine(savingsAccount.DisplayAccountInfo());

                // SavingsAccount: Withdraw exceeding limit
                Console.WriteLine("\nSavingsAccount: Attempting to withdraw $900 (exceeding withdrawal limit)...");
                savingsAccount.Withdraw(900);
                Console.WriteLine(savingsAccount.DisplayAccountInfo());

                // SavingsAccount: Exceeding maximum number of withdrawals per month
                Console.WriteLine("\nSavingsAccount: Exceeding maximum number of withdrawals per month...");
                for (int i = 0; i < 7; i++)
                {
                    Console.WriteLine($"Attempting to withdraw $50 (withdrawal {i + 1})...");
                    savingsAccount.Withdraw(50);
                    Console.WriteLine(savingsAccount.DisplayAccountInfo());
                }
            }
            else if (account is MoneyMarketAccount moneyMarketAccount)
            {
                // MoneyMarketAccount: Withdraw within minimum balance
                Console.WriteLine("\nMoneyMarketAccount: Attempting to withdraw $1000 (maintaining minimum balance)...");
                moneyMarketAccount.Withdraw(1000);
                Console.WriteLine(moneyMarketAccount.DisplayAccountInfo());

                // MoneyMarketAccount: Withdraw exceeding minimum balance
                Console.WriteLine("\nMoneyMarketAccount: Attempting to withdraw $1900 (exceeding minimum balance)...");
                moneyMarketAccount.Withdraw(1900);
                Console.WriteLine(moneyMarketAccount.DisplayAccountInfo());
            }
        }
    }
}