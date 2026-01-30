using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; } = true;
        public Department? Department { get; set; }
        public List<ProjectAssignment> ProjectAssignments { get; set; } = new();



    }
}
