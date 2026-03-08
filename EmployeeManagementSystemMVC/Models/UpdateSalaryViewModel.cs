using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemMVC.Models
{
    public class UpdateSalaryViewModel
    {
        public int EmployeeId { get; set; }

        public string? EmployeeName { get; set; } 

        [Display(Name = "Current Salary")]
        public decimal CurrentSalary { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "New salary cannot be negative.")]
        [Display(Name = "New Salary")]
        public decimal NewSalary { get; set; }
    }
}