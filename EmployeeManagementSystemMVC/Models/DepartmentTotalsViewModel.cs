namespace EmployeeManagementSystemMVC.Models
{
    public class DepartmentTotalsViewModel
    {
        public string DepartmentName { get; set; } = null!;
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal AverageSalary { get; set; }
    }
}