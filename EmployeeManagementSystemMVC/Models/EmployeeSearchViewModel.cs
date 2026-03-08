using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemMVC.Models
{
    public class EmployeeSearchViewModel
    {
        public string? SearchName { get; set; }

        public int? DepartmentId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Minimum salary cannot be negative.")]
        public decimal? MinSalary { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Maximum salary cannot be negative.")]
        public decimal? MaxSalary { get; set; }

        public bool ActiveOnly { get; set; } = true;

        public List<Employee> Employees { get; set; } = new();

        public SelectList? Departments { get; set; }
    }
}