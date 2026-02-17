using System;

namespace Files_M2;

public interface IYearlyReportGenerator
{
    void GeneratePreviousYearReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
    void GenerateCurrentYearToDateReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
    void GenerateLast365DaysReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
}
