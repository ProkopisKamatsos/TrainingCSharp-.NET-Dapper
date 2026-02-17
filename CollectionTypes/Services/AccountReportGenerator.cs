using System;

namespace Data_M2;

public class AccountReportGenerator : IMonthlyReportGenerator, IQuarterlyReportGenerator, IYearlyReportGenerator
{
    private readonly IBankAccount _account;

    public AccountReportGenerator(IBankAccount account)
    {
        _account = account;
    }

    public void GenerateMonthlyReport()
    {
        Console.WriteLine($"Generating monthly report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating monthly report based on transaction history
    }

    public void GenerateCurrentMonthToDateReport()
    {
        Console.WriteLine($"Generating current month-to-date report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating current month-to-date report based on transaction history
    }

    public void GeneratePrevious30DayReport()
    {
        Console.WriteLine($"Generating previous month report for for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating previous month report based on transaction history
    }

    public void GenerateQuarterlyReport()
    {
        Console.WriteLine($"Generating quarterly report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating quarterly report based on transaction history
    }

    public void GeneratePreviousYearReport()
    {
        Console.WriteLine($"Generating previous year report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating previous year report based on transaction history
    }

    public void GenerateCurrentYearToDateReport()
    {
        Console.WriteLine($"Generating current year-to-date report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating current year-to-date report based on transaction history
    }

    public void GenerateLast365DaysReport()
    {
        Console.WriteLine($"Generating last 365 days report for {_account.AccountType} account number: {_account.AccountNumber}");
        // Logic for generating last 365 days report based on transaction history
    }
}
