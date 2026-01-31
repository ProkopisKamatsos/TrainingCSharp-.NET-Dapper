using System;

namespace Data_M2;

public interface IYearlyReportGenerator
{
    void GeneratePreviousYearReport(); // Generates a report for the previous year
    void GenerateCurrentYearToDateReport(); // Generates a report for the current year up to the current date
    void GenerateLast365DaysReport(); // Generates a report for the previous 365 days
}
