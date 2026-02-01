using System;

namespace Reuse_M2;

public interface IQuarterlyReportGenerator
{
    void GenerateQuarterlyReport(); // Generates a report for a complete three-month period (Jan-Mar, Apr-Jun, Jul-Sep, Oct-Dec)
}
