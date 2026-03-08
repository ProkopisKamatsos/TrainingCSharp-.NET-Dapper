using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string? Location { get; set; }

        public int? ManagerId { get; set; }

        public Employee? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}