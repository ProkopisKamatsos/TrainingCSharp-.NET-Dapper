using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemMVC.Models
{
    public class AssignmentCreateViewModel
    {
        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = null!;

        public SelectList? Employees { get; set; }
        public SelectList? Projects { get; set; }
    }
}