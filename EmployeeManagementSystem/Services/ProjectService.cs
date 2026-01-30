using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

public class ProjectService
{
    private readonly ProjectRepository _projectRepo;

    public ProjectService(ProjectRepository projectRepo)
    {
        _projectRepo = projectRepo;
    }

    public List<Project> GetAllProjects()
        => _projectRepo.GetAll();
    public async Task<List<Project>> GetAllProjectsAsync()
        => await _projectRepo.GetAllAsync();

    public Project? GetProjectById(int id)
        => _projectRepo.GetById(id);
    public async Task<Project?> GetProjectByIdAsync(int id)
        => await _projectRepo.GetByIdAsync(id);

    public Project CreateProject(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
            throw new Exception("Project name is required");

        project.Name = project.Name.Trim();

        if (_projectRepo.ExistsByName(project.Name))
            throw new Exception("Project name already exists");

        if (project.Budget < 0)
            throw new Exception("Budget cannot be negative");

        if (project.EndDate.HasValue && project.EndDate.Value.Date < project.StartDate.Date)
            throw new Exception("EndDate cannot be before StartDate");

        return _projectRepo.Insert(project);
    }
    public async Task<Project> CreateProjectAsync(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.Name))
            throw new Exception("Project name is required");
        project.Name = project.Name.Trim();
        if (await _projectRepo.ExistsByNameAsync(project.Name))
            throw new Exception("Project name already exists");
        if (project.Budget < 0)
            throw new Exception("Budget cannot be negative");
        if (project.EndDate.HasValue && project.EndDate.Value.Date < project.StartDate.Date)
            throw new Exception("EndDate cannot be before StartDate");
        return await _projectRepo.InsertAsync(project);
    }
    public Project UpdateProject(Project project)
    {
        if (project.Id <= 0)
            throw new Exception("Invalid project Id");

        var existing = _projectRepo.GetById(project.Id);
        if (existing is null)
            throw new Exception("Project not found");

        if (string.IsNullOrWhiteSpace(project.Name))
            throw new Exception("Project name is required");

        project.Name = project.Name.Trim();

        if (_projectRepo.ExistsByName(project.Name, project.Id))
            throw new Exception("Project name already exists");

        if (project.Budget < 0)
            throw new Exception("Budget cannot be negative");

        if (project.EndDate.HasValue && project.EndDate.Value.Date < project.StartDate.Date)
            throw new Exception("EndDate cannot be before StartDate");

        return _projectRepo.Update(project);
    }
    public async Task<Project> UpdateProjectAsync(Project project)
    {
        if (project.Id <= 0)
            throw new Exception("Invalid project Id");
        var existing = await _projectRepo.GetByIdAsync(project.Id);
        if (existing is null)
            throw new Exception("Project not found");
        if (string.IsNullOrWhiteSpace(project.Name))
            throw new Exception("Project name is required");
        project.Name = project.Name.Trim();
        if (await _projectRepo.ExistsByNameAsync(project.Name, project.Id))
            throw new Exception("Project name already exists");
        if (project.Budget < 0)
            throw new Exception("Budget cannot be negative");
        if (project.EndDate.HasValue && project.EndDate.Value.Date < project.StartDate.Date)
            throw new Exception("EndDate cannot be before StartDate");
        return await _projectRepo.UpdateAsync(project);
    }

    public void DeleteProject(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid project Id");

        var existing = _projectRepo.GetById(id);
        if (existing is null)
            throw new Exception("Project not found");

        _projectRepo.Delete(id);
    }
    public async Task DeleteProjectAsync(int id)
    {
        if (id <= 0)
            throw new Exception("Invalid project Id");
        var existing = await _projectRepo.GetByIdAsync(id);
        if (existing is null)
            throw new Exception("Project not found");
        await _projectRepo.DeleteAsync(id);
    }
}
