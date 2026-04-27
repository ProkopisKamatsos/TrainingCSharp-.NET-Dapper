var levels = new List<Level>
{
    new Level { Id = 1, Name = "Junior" },
    new Level { Id = 2, Name = "Mid"    },
    new Level { Id = 3, Name = "Senior" }
};

var statuses = new List<Status>
{
    new Status { Id = 1, Name = "Active"    },
    new Status { Id = 2, Name = "Completed" },
    new Status { Id = 3, Name = "Cancelled" }
};

var departments = new List<Department>
{
    new Department { Id = 1, Name = "Engineering", ManagerId = 1 },
    new Department { Id = 2, Name = "Design",      ManagerId = 3 },
    new Department { Id = 3, Name = "Marketing",   ManagerId = 5 }
};

var employees = new List<Employee>
{
    new Employee { Id = 1, Name = "Alice",  Email = "alice@co.com",  Salary = 3500, IsActive = true,  HireDate = new DateTime(2020,1,15),  DepartmentId = 1, LevelId = 3 },
    new Employee { Id = 2, Name = "Bob",    Email = "bob@co.com",    Salary = 2200, IsActive = true,  HireDate = new DateTime(2022,3,10),  DepartmentId = 1, LevelId = 1 },
    new Employee { Id = 3, Name = "Carol",  Email = "carol@co.com",  Salary = 3000, IsActive = true,  HireDate = new DateTime(2021,6,1),   DepartmentId = 2, LevelId = 3 },
    new Employee { Id = 4, Name = "Dave",   Email = "dave@co.com",   Salary = 2500, IsActive = true,  HireDate = new DateTime(2022,9,20),  DepartmentId = 2, LevelId = 2 },
    new Employee { Id = 5, Name = "Eve",    Email = "eve@co.com",    Salary = 2800, IsActive = true,  HireDate = new DateTime(2021,11,5),  DepartmentId = 3, LevelId = 2 },
    new Employee { Id = 6, Name = "Frank",  Email = "frank@co.com",  Salary = 1900, IsActive = false, HireDate = new DateTime(2023,2,1),   DepartmentId = 1, LevelId = 1 }
};

var projects = new List<Project>
{
    new Project { Id = 1, Name = "Website Redesign", Budget = 50000, ManagerId = 1, StatusId = 1, StartDate = new DateTime(2024,1,1),  Deadline = new DateTime(2024,6,1)  },
    new Project { Id = 2, Name = "Mobile App",       Budget = 80000, ManagerId = 3, StatusId = 1, StartDate = new DateTime(2024,3,1),  Deadline = new DateTime(2024,12,1) },
    new Project { Id = 3, Name = "Data Migration",   Budget = 20000, ManagerId = 1, StatusId = 2, StartDate = new DateTime(2023,6,1),  Deadline = new DateTime(2023,12,1) }
};

var employeeProjects = new List<EmployeeProject>
{
    new EmployeeProject { EmployeeId = 1, ProjectId = 1, Role = "Tech Lead",  AssignedAt = new DateTime(2024,1,1)  },
    new EmployeeProject { EmployeeId = 2, ProjectId = 1, Role = "Developer",  AssignedAt = new DateTime(2024,1,5)  },
    new EmployeeProject { EmployeeId = 3, ProjectId = 1, Role = "Designer",   AssignedAt = new DateTime(2024,1,5)  },
    new EmployeeProject { EmployeeId = 3, ProjectId = 2, Role = "Tech Lead",  AssignedAt = new DateTime(2024,3,1)  },
    new EmployeeProject { EmployeeId = 4, ProjectId = 2, Role = "Designer",   AssignedAt = new DateTime(2024,3,5)  },
    new EmployeeProject { EmployeeId = 2, ProjectId = 2, Role = "Developer",  AssignedAt = new DateTime(2024,3,10) },
    new EmployeeProject { EmployeeId = 1, ProjectId = 3, Role = "Tech Lead",  AssignedAt = new DateTime(2023,6,1)  },
    new EmployeeProject { EmployeeId = 5, ProjectId = 3, Role = "Analyst",    AssignedAt = new DateTime(2023,6,5)  }
};
// Βρες τους υπαλλήλους που δουλεύουν
// σε περισσότερα από ένα project.
// Εμφάνισε όνομα υπαλλήλου και 
// αριθμό projects.
var result = employees
    .Select(e => new
    {
        EmployeeName = e.Name,
        ProjectCount = e.EmployeeProjects.Count
    })
    .Where(x => x.ProjectCount > 1);
public class Level
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Employee> Employees { get; set; } = new();
}

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Project> Projects { get; set; } = new();
}

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ManagerId { get; set; }
    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
    public DateTime HireDate { get; set; }
    public int DepartmentId { get; set; }
    public int LevelId { get; set; }
    public List<EmployeeProject> EmployeeProjects { get; set; } = new();
}

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }
    public int ManagerId { get; set; }
    public int StatusId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime Deadline { get; set; }
    public List<EmployeeProject> EmployeeProjects { get; set; } = new();
}

public class EmployeeProject
{
    public int EmployeeId { get; set; }
    public int ProjectId { get; set; }
    public string Role { get; set; }
    public DateTime AssignedAt { get; set; }
}