using EmployeeManagementSystemMVC.Data;
using EmployeeManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemMVC.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var assignments = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .ThenInclude(e => e.Department)
                .Include(ep => ep.Project)
                .OrderBy(ep => ep.Project!.Name)
                .ThenBy(ep => ep.Employee!.LastName)
                .ToListAsync();

            return View(assignments);
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            var model = new AssignmentCreateViewModel
            {
                Employees = new SelectList(
                    await _context.Employees
                        .Where(e => e.IsActive)
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                        .Select(e => new
                        {
                            e.Id,
                            FullName = e.FirstName + " " + e.LastName
                        })
                        .ToListAsync(),
                    "Id",
                    "FullName"
                ),
                Projects = new SelectList(
                    await _context.Projects
                        .OrderBy(p => p.Name)
                        .ToListAsync(),
                    "Id",
                    "Name"
                )
            };

            return View(model);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignmentCreateViewModel model)
        {
            bool employeeExists = await _context.Employees
                .AnyAsync(e => e.Id == model.EmployeeId && e.IsActive);

            if (!employeeExists)
            {
                ModelState.AddModelError("EmployeeId", "Selected employee does not exist or is inactive.");
            }

            bool projectExists = await _context.Projects
                .AnyAsync(p => p.Id == model.ProjectId);

            if (!projectExists)
            {
                ModelState.AddModelError("ProjectId", "Selected project does not exist.");
            }

            bool assignmentExists = await _context.EmployeeProjects
                .AnyAsync(ep => ep.EmployeeId == model.EmployeeId && ep.ProjectId == model.ProjectId);

            if (assignmentExists)
            {
                ModelState.AddModelError(string.Empty, "This employee is already assigned to this project.");
            }

            if (string.IsNullOrWhiteSpace(model.Role))
            {
                ModelState.AddModelError("Role", "Role is required.");
            }

            if (ModelState.IsValid)
            {
                var assignment = new EmployeeProject
                {
                    EmployeeId = model.EmployeeId,
                    ProjectId = model.ProjectId,
                    Role = model.Role
                };

                _context.EmployeeProjects.Add(assignment);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Assignment created successfully.";
                return RedirectToAction(nameof(Index));
            }

            model.Employees = new SelectList(
                await _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .Select(e => new
                    {
                        e.Id,
                        FullName = e.FirstName + " " + e.LastName
                    })
                    .ToListAsync(),
                "Id",
                "FullName",
                model.EmployeeId
            );

            model.Projects = new SelectList(
                await _context.Projects
                    .OrderBy(p => p.Name)
                    .ToListAsync(),
                "Id",
                "Name",
                model.ProjectId
            );

            return View(model);
        }
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? employeeId, int? projectId)
        {
            if (employeeId == null || projectId == null)
            {
                return NotFound();
            }

            var assignment = await _context.EmployeeProjects
                .Include(ep => ep.Employee)
                .Include(ep => ep.Project)
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId, int projectId)
        {
            var assignment = await _context.EmployeeProjects
                .FirstOrDefaultAsync(ep => ep.EmployeeId == employeeId && ep.ProjectId == projectId);

            if (assignment == null)
            {
                return NotFound();
            }

            _context.EmployeeProjects.Remove(assignment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Assignment removed successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}