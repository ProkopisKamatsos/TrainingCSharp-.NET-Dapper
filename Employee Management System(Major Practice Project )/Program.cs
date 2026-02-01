using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Z.Dapper.Plus;
using Microsoft.Data.SqlClient;

DapperPlusManager.Entity<Employee>()
    .Table("Employees")
    .Identity(x => x.Id); 


var cs = "Server=localhost\\SQLEXPRESS;Database=CompanyDB;Trusted_Connection=True;TrustServerCertificate=True;";

var factory = new DbConnectionFactory(cs);

var employeeRepo = new EmployeeRepository(factory);
var departmentRepo = new DepartmentRepository(factory);
var employeeService = new EmployeeService(employeeRepo, departmentRepo);
var departmentService = new DepartmentService(departmentRepo, employeeRepo);
var projectRepo = new ProjectRepository(factory);
var projectService = new ProjectService(projectRepo);
var employeeProjectRepo = new EmployeeProjectRepository(factory);
var employeeProjectService = new EmployeeProjectService(employeeRepo, projectRepo, employeeProjectRepo);


while (true)
{
    Console.WriteLine();
    Console.WriteLine("1. List all employees");
    Console.WriteLine("2. Find employee and dept by ID");
    Console.WriteLine("3. Create employee");
    Console.WriteLine("4. Update an existing employee");
    Console.WriteLine("5. Delete (deactivate) an employee");
    Console.WriteLine("6. Create department");
    Console.WriteLine("7. List all departments");
    Console.WriteLine("8. Update department");
    Console.WriteLine("9. Delete department");
    Console.WriteLine("10. Create project");
    Console.WriteLine("11. List all projects");
    Console.WriteLine("12. Find project by ID");
    Console.WriteLine("13. Update project");
    Console.WriteLine("14. Delete project");
    Console.WriteLine("15. Search employee by filters");
    Console.WriteLine("16. View department totals (report)");
    Console.WriteLine("17. Assign employee to project");
    Console.WriteLine("18. View employee with projects");
    Console.WriteLine("19. View department totals (Stored Procedure)");
    Console.WriteLine("20. Update employee salary (Transaction)");
    Console.WriteLine("21. Bulk import demo employees (Dapper Plus)");
    Console.WriteLine("0. Exit");

    Console.Write("Choose: ");
    var choice = Console.ReadLine();

    if (choice == "0")
        break;

    switch (choice)
    {
        case "1":
            {
                var employees = await employeeRepo.GetAllAsync();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.Id} - {e.FirstName} {e.LastName} - Department:{e.DepartmentId} - Active: {e.IsActive}-Salary: {e.Salary}");
                }

                break;
            }

        case "2":
            {
                Console.Write("Employee Id: ");
                int.TryParse(Console.ReadLine(), out int employeeId);

                try
                {
                    var employee = await employeeService.GetEmployeeWithDepartmentByIdAsync(employeeId);

                    Console.WriteLine($"{employee.Id} - {employee.FirstName} {employee.LastName}");
                    Console.WriteLine($"Email: {employee.Email}");
                    Console.WriteLine($"Salary: {employee.Salary}");
                    Console.WriteLine($"Department: {employee.Department?.Name} | Location: {employee.Department?.Location}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }


        case "3":
            {
                Console.Write("First name: ");
                var firstName = Console.ReadLine();

                Console.Write("Last name: ");
                var lastName = Console.ReadLine();

                Console.Write("Email: ");
                var email = Console.ReadLine();

                Console.Write("Department Id: ");
                int.TryParse(Console.ReadLine(), out int departmentId);

                Console.Write("Salary: ");
                decimal.TryParse(Console.ReadLine(), out decimal salary);

                var employee = new Employee
                {
                    FirstName = firstName ?? "",
                    LastName = lastName ?? "",
                    Email = email ?? "",
                    DepartmentId = departmentId,
                    Salary = salary,
                    HireDate = DateTime.Now,
                    IsActive = true
                };

                try
                {
                    var createdEmployee = await employeeService.CreateEmployeeAsync(employee);

                    Console.WriteLine($"Employee created with Id: {createdEmployee.Id}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        case "4":
            {
                Console.Write("Employee Id to update: ");
                int.TryParse(Console.ReadLine(), out int employeeId);

                Console.Write("First name: ");
                var firstName = Console.ReadLine();

                Console.Write("Last name: ");
                var lastName = Console.ReadLine();

                Console.Write("Email: ");
                var email = Console.ReadLine();

                Console.Write("Department Id: ");
                int.TryParse(Console.ReadLine(), out int departmentId);

                Console.Write("Salary: ");
                decimal.TryParse(Console.ReadLine(), out decimal salary);

                var employee = new Employee
                {
                    Id = employeeId,
                    FirstName = firstName ?? "",
                    LastName = lastName ?? "",
                    Email = email ?? "",
                    DepartmentId = departmentId,
                    Salary = salary,
                    IsActive = true
                };

                try
                {
                    var updatedEmployee = await employeeService.UpdateEmployeeAsync(employee);

                    Console.WriteLine($"Employee updated: {updatedEmployee.Id} - {updatedEmployee.FirstName} {updatedEmployee.LastName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        case "5":
            {
                Console.Write("Employee Id to delete: ");
                int.TryParse(Console.ReadLine(), out int id);

                try
                {
                    await employeeService.DeleteEmployeeAsync(id);
                    Console.WriteLine("Employee deactivated successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        case "6":
            {
                Console.Write("Department name: ");
                var name = Console.ReadLine();

                Console.Write("Location (optional): ");
                var location = Console.ReadLine();

                Console.Write("Manager Id (optional, 0 if none): ");
                int.TryParse(Console.ReadLine(), out int managerId);

                var department = new Department
                {
                    Name = name ?? "",
                    Location = location ?? "",
                    ManagerId = managerId
                };

                try
                {
                    var created = await departmentService.CreateDepartmentAsync(department);
                    Console.WriteLine($"Department created: {created.Id} - {created.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        case "7":
            {
                var departments = await departmentService.GetAllDepartmentsAsync();

                if (departments.Count == 0)
                {
                    Console.WriteLine("No departments found");
                }
                else
                {
                    foreach (var d in departments)
                    {
                        Console.WriteLine(
                            $"{d.Id} - {d.Name} | Location: {d.Location} | ManagerId: {d.ManagerId}"
                        );
                    }
                }

                break;
            }

        case "8":
            {
                var departments = await departmentService.GetAllDepartmentsAsync();
                foreach (var d in departments)
                    Console.WriteLine($"{d.Id} - {d.Name}");

                Console.Write("Department Id to update: ");
                int.TryParse(Console.ReadLine(), out int deptId);

                Console.Write("New name: ");
                var name = Console.ReadLine();

                Console.Write("New location (optional): ");
                var location = Console.ReadLine();

                Console.Write("New manager id (optional, 0 if none): ");
                int.TryParse(Console.ReadLine(), out int managerId);

                var dept = new Department
                {
                    Id = deptId,
                    Name = name ?? "",
                    Location = location ?? "",
                    ManagerId = managerId
                };

                try
                {
                    var updated = await departmentService.UpdateDepartmentAsync(dept);
                    Console.WriteLine($"Department updated: {updated.Id} - {updated.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        case "9":
            {
                Console.Write("Department Id to delete: ");
                int.TryParse(Console.ReadLine(), out int deptId);

                try
                {
                    await departmentService.DeleteDepartmentAsync(deptId);
                    Console.WriteLine("Department deleted successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "10":
            {
                Console.Write("Project name: ");
                var name = Console.ReadLine();

                Console.Write("Start date (YYYY-MM-DD): ");
                DateTime.TryParse(Console.ReadLine(), out var startDate);

                Console.Write("End date (YYYY-MM-DD) (optional, empty for none): ");
                var endInput = Console.ReadLine();
                DateTime? endDate = null;
                if (!string.IsNullOrWhiteSpace(endInput) && DateTime.TryParse(endInput, out var parsedEnd))
                    endDate = parsedEnd;

                Console.Write("Budget: ");
                decimal.TryParse(Console.ReadLine(), out var budget);

                var project = new Project
                {
                    Name = name ?? "",
                    StartDate = startDate,
                    EndDate = endDate,
                    Budget = budget
                };

                try
                {
                    var created = await projectService.CreateProjectAsync(project);
                    Console.WriteLine($"Project created: {created.Id} - {created.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "11":
            {
                var projects = await projectService.GetAllProjectsAsync();
                if (projects.Count == 0)
                {
                    Console.WriteLine("No projects found");
                }
                else
                {
                    foreach (var p in projects)
                        Console.WriteLine($"{p.Id} - {p.Name} | {p.StartDate:yyyy-MM-dd} → {(p.EndDate.HasValue ? p.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")} | Budget: {p.Budget}");
                }
                break;
            }
        case "12":
            {
                Console.Write("Project Id: ");
                int.TryParse(Console.ReadLine(), out var id);

                var project = await projectService.GetProjectByIdAsync(id);
                if (project is null)
                    Console.WriteLine("Project not found");
                else
                    Console.WriteLine($"{project.Id} - {project.Name} | {project.StartDate:yyyy-MM-dd} → {(project.EndDate.HasValue ? project.EndDate.Value.ToString("yyyy-MM-dd") : "N/A")} | Budget: {project.Budget}");

                break;
            }
        case "13":
            {
                Console.Write("Project Id to update: ");
                int.TryParse(Console.ReadLine(), out var id);

                Console.Write("New name: ");
                var name = Console.ReadLine();

                Console.Write("Start date (YYYY-MM-DD): ");
                DateTime.TryParse(Console.ReadLine(), out var startDate);

                Console.Write("End date (YYYY-MM-DD) (optional, empty for none): ");
                var endInput = Console.ReadLine();
                DateTime? endDate = null;
                if (!string.IsNullOrWhiteSpace(endInput) && DateTime.TryParse(endInput, out var parsedEnd))
                    endDate = parsedEnd;

                Console.Write("Budget: ");
                decimal.TryParse(Console.ReadLine(), out var budget);

                var project = new Project
                {
                    Id = id,
                    Name = name ?? "",
                    StartDate = startDate,
                    EndDate = endDate,
                    Budget = budget
                };

                try
                {
                    var updated = await projectService.UpdateProjectAsync(project);
                    Console.WriteLine($"Project updated: {updated.Id} - {updated.Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "14":
            {
                Console.Write("Project Id to delete: ");
                int.TryParse(Console.ReadLine(), out var id);

                try
                {
                    await projectService.DeleteProjectAsync(id);
                    Console.WriteLine("Project deleted successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "15":
            {
                Console.Write("Name (optional): ");
                var name = Console.ReadLine();

                Console.Write("Department Id (optional, empty to skip): ");
                var deptInput = Console.ReadLine();
                int? deptId = null;
                if (!string.IsNullOrWhiteSpace(deptInput) && int.TryParse(deptInput, out var d))
                    deptId = d;

                Console.Write("Salary From (optional, empty to skip): ");
                var fromInput = Console.ReadLine();
                decimal? salaryFrom = null;
                if (!string.IsNullOrWhiteSpace(fromInput) && decimal.TryParse(fromInput, out var from))
                    salaryFrom = from;

                Console.Write("Salary To (optional, empty to skip): ");
                var toInput = Console.ReadLine();
                decimal? salaryTo = null;
                if (!string.IsNullOrWhiteSpace(toInput) && decimal.TryParse(toInput, out var to))
                    salaryTo = to;

                var criteria = new EmployeeSearchCriteria
                {
                    Name = string.IsNullOrWhiteSpace(name) ? null : name,
                    DepartmentId = deptId,
                    SalaryFrom = salaryFrom,
                    SalaryTo = salaryTo
                };

                try
                {
                    var results = await employeeService.SearchEmployeesAsync(criteria);

                    if (results.Count == 0)
                    {
                        Console.WriteLine("No employees found");
                    }
                    else
                    {
                        foreach (var e in results)
                            Console.WriteLine($"{e.Id} - {e.FirstName} {e.LastName} - Dept:{e.DepartmentId} - Salary:{e.Salary}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "16":
            {
                Console.Write("Enter department id: ");
                if (!int.TryParse(Console.ReadLine(), out var deptId))
                {
                    Console.WriteLine("Invalid input.");
                    break;
                }

                try
                {
                    var t = await departmentService.GetDepartmentTotalsByIdAsync(deptId);

                    Console.WriteLine("Department Totals:");
                    Console.WriteLine($"ID: {t.DepartmentId}");
                    Console.WriteLine($"Name: {t.DepartmentName}");
                    Console.WriteLine($"Total Employees: {t.TotalEmployeeCount}");
                    Console.WriteLine($"Active Employees: {t.ActiveEmployeeCount}");
                    Console.WriteLine($"Inactive Employees: {t.InactiveEmployeeCount}");
                    Console.WriteLine($"Active Total Salary: {t.ActiveTotalSalary:N2}");
                    Console.WriteLine($"Active Avg Salary: {t.ActiveAverageSalary:N2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }



        case "17":
            {
                Console.Write("Employee Id: ");
                int.TryParse(Console.ReadLine(), out var empId);

                Console.Write("Project Id: ");
                int.TryParse(Console.ReadLine(), out var projId);
                Console.Write("Role: ");
                var role = Console.ReadLine();

                try
                {
                    await employeeProjectService.AssignEmployeeToProjectAsync(empId, projId, role ?? "");
                    Console.WriteLine("Assigned successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                break;
            }
        case "18":
            {
                Console.Write("Employee Id: ");
                int.TryParse(Console.ReadLine(), out var empId);

                var emp = await employeeRepo.GetEmployeeWithProjectsByIdAsync(empId);

                if (emp is null)
                {
                    Console.WriteLine("Employee not found");
                    break;
                }

                Console.WriteLine($"{emp.Id} - {emp.FirstName} {emp.LastName}");

                if (emp.ProjectAssignments.Count == 0)
                {
                    Console.WriteLine("No projects assigned");
                }
                else
                {
                    Console.WriteLine("Projects:");
                    foreach (var a in emp.ProjectAssignments)
                        Console.WriteLine($"- {a.Project.Id}: {a.Project.Name} (Role: {a.Role})");
                }

                break;
            }
        case "19":
            {
                Console.Write("Enter department id: ");
                if (!int.TryParse(Console.ReadLine(), out var deptId))
                {
                    Console.WriteLine("Invalid input.");
                    break;
                }

                try
                {
                    var t = await departmentService.GetDepartmentTotalsById_StoredProcedureAsync(deptId);

                    Console.WriteLine("Department Totals (Stored Procedure):");
                    Console.WriteLine($"ID: {t.DepartmentId}");
                    Console.WriteLine($"Name: {t.DepartmentName}");
                    Console.WriteLine($"Total Employees: {t.TotalEmployeeCount}");
                    Console.WriteLine($"Active Employees: {t.ActiveEmployeeCount}");
                    Console.WriteLine($"Inactive Employees: {t.InactiveEmployeeCount}");
                    Console.WriteLine($"Active Total Salary: {t.ActiveTotalSalary:N2}");
                    Console.WriteLine($"Active Avg Salary: {t.ActiveAverageSalary:N2}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }
        case "20":
            {
                Console.Write("Enter employee id: ");
                if (!int.TryParse(Console.ReadLine(), out var empId))
                {
                    Console.WriteLine("Invalid employee id.");
                    break;
                }

                Console.Write("Enter new salary: ");
                if (!decimal.TryParse(Console.ReadLine(), out var newSalary))
                {
                    Console.WriteLine("Invalid salary.");
                    break;
                }

                try
                {
                    await employeeService.UpdateEmployeeSalaryAsync(empId, newSalary);
                    Console.WriteLine("Salary updated successfully (transaction committed).");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message} (transaction rolled back).");
                }

                break;
            }
        case "21":
            {
                Console.Write("How many employees to import? ");
                if (!int.TryParse(Console.ReadLine(), out var n) || n <= 0)
                {
                    Console.WriteLine("Invalid number.");
                    break;
                }

                Console.Write("Department Id for all (e.g. 1): ");
                if (!int.TryParse(Console.ReadLine(), out var deptId) || deptId <= 0)
                {
                    Console.WriteLine("Invalid department id.");
                    break;
                }

                var employees = new List<Employee>();
                for (int i = 0; i < n; i++)
                {
                    employees.Add(new Employee
                    {
                        FirstName = "Demo",
                        LastName = $"User{i + 1}",
                        Email = $"demo.user{i + 1}@example.com",
                        Phone = null,
                        DepartmentId = deptId,
                        Salary = 1000 + i,
                        HireDate = DateTime.Now,
                        IsActive = true
                    });
                }

                try
                {
                    await employeeService.BulkImportEmployeesAsync(employees);
                    Console.WriteLine($"Bulk import completed: {n} employees inserted.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                break;
            }

        default:
            Console.WriteLine("Invalid option");
            break;
    }


}
