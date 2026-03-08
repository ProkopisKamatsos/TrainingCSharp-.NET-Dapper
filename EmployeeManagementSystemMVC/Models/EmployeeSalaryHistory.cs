using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemMVC.Models
{
    public class EmployeeSalaryHistory
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal OldSalary { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NewSalary { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
    }
}