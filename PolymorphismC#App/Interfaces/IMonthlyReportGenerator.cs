using System;

namespace Reuse_M2;

public interface IMonthlyReportGenerator
{
    void GenerateMonthlyReport(); // Generates a report for a complete month
    void GenerateCurrentMonthToDateReport(); // Generates a report for the current month up to the current date
    void GeneratePrevious30DayReport(); // Generates a report for the previous 30 day period
}
