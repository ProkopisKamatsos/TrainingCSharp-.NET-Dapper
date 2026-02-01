using EmployeeManagementSystem.Data;

namespace EmployeeManagementSystem.Services;

public class EmployeeProjectService
{
    private readonly EmployeeRepository _employeeRepo;
    private readonly ProjectRepository _projectRepo;
    private readonly EmployeeProjectRepository _employeeProjectRepo;

    public EmployeeProjectService(
        EmployeeRepository employeeRepo,
        ProjectRepository projectRepo,
        EmployeeProjectRepository employeeProjectRepo)
    {
        _employeeRepo = employeeRepo;
        _projectRepo = projectRepo;
        _employeeProjectRepo = employeeProjectRepo;
    }

    public void AssignEmployeeToProject(int employeeId, int projectId, string role)
    {
        if (employeeId <= 0) throw new Exception("Invalid employee Id");
        if (projectId <= 0) throw new Exception("Invalid project Id");
        if (string.IsNullOrWhiteSpace(role)) throw new Exception("Role is required");

        var emp = _employeeRepo.GetById(employeeId);
        if (emp is null) throw new Exception("Employee not found");
        if (!emp.IsActive) throw new Exception("Employee is inactive");

        var proj = _projectRepo.GetById(projectId);
        if (proj is null) throw new Exception("Project not found");

        _employeeProjectRepo.Assign(employeeId, projectId, role.Trim());
    }
    public async Task AssignEmployeeToProjectAsync(int employeeId, int projectId, string role)
    {
        if (employeeId <= 0) throw new Exception("Invalid employee Id");
        if (projectId <= 0) throw new Exception("Invalid project Id");
        if (string.IsNullOrWhiteSpace(role)) throw new Exception("Role is required");
        var emp = await _employeeRepo.GetByIdAsync(employeeId);
        if (emp is null) throw new Exception("Employee not found");
        if (!emp.IsActive) throw new Exception("Employee is inactive");
        var proj = await _projectRepo.GetByIdAsync(projectId);
        if (proj is null) throw new Exception("Project not found");
        await _employeeProjectRepo.AssignAsync(employeeId, projectId, role.Trim());
    }

}
