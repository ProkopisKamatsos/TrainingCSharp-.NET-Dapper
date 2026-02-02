using System;

namespace Files_M1;

public interface IQuarterlyReportGenerator
{
    void GenerateQuarterlyReport(BankCustomer bankCustomer, int accountNumber, DateOnly reportDate);
}
