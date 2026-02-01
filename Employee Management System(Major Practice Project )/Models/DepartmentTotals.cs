namespace EmployeeManagementSystem.Models;

public class DepartmentTotals
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = "";

    public int TotalEmployeeCount { get; set; }
    public int ActiveEmployeeCount { get; set; }
    public int InactiveEmployeeCount { get; set; }

    public decimal ActiveTotalSalary { get; set; }
    public decimal ActiveAverageSalary { get; set; }
}
