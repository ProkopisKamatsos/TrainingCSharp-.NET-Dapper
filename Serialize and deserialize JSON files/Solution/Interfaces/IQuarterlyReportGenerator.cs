using System;

namespace Files_M2;

public interface IQuarterlyReportGenerator
{
    void GenerateQuarterlyReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
}
