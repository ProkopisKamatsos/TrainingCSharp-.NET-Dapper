using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemMVC.Models
{
    public class EmployeeProject
    {
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = null!;
    }
}