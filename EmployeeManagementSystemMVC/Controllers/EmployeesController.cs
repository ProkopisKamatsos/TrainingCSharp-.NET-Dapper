using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemMVC.Data;
using EmployeeManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementSystemMVC.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string? searchName, int? departmentId, decimal? minSalary, decimal? maxSalary, bool activeOnly = true)
        {
            if (minSalary < 0)
            {
                ModelState.AddModelError("MinSalary", "Minimum salary cannot be negative.");
            }

            if (maxSalary < 0)
            {
                ModelState.AddModelError("MaxSalary", "Maximum salary cannot be negative.");
            }

            if (minSalary.HasValue && maxSalary.HasValue && minSalary > maxSalary)
            {
                ModelState.AddModelError(string.Empty, "Minimum salary cannot be greater than maximum salary.");
            }

            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                query = query.Where(e =>
                    (e.FirstName + " " + e.LastName).Contains(searchName) ||
                    e.FirstName.Contains(searchName) ||
                    e.LastName.Contains(searchName));
            }

            if (departmentId.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentId.Value);
            }

            if (minSalary.HasValue)
            {
                query = query.Where(e => e.Salary >= minSalary.Value);
            }

            if (maxSalary.HasValue)
            {
                query = query.Where(e => e.Salary <= maxSalary.Value);
            }

            if (activeOnly)
            {
                query = query.Where(e => e.IsActive);
            }

            var model = new EmployeeSearchViewModel
            {
                SearchName = searchName,
                DepartmentId = departmentId,
                MinSalary = minSalary,
                MaxSalary = maxSalary,
                ActiveOnly = activeOnly,
                Employees = await query.ToListAsync(),
                Departments = new SelectList(await _context.Departments.ToListAsync(), "Id", "Name")
            };

            return View(model);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,DepartmentId,Salary,HireDate")] Employee employee)
        {
            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
            }

            if (employee.Salary < 0)
            {
                ModelState.AddModelError("Salary", "Salary cannot be negative.");
            }

            if (employee.HireDate > DateTime.Today)
            {
                ModelState.AddModelError("HireDate", "Hire date cannot be in the future.");
            }

            if (ModelState.IsValid)
            {
                employee.IsActive = true;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            if (!employee.IsActive)
            {
                TempData["ErrorMessage"] = "Inactive employees cannot be edited.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,DepartmentId,Salary,HireDate")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            bool emailExists = await _context.Employees
                .AnyAsync(e => e.Email == employee.Email && e.Id != employee.Id);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "An employee with this email already exists.");
            }

            if (employee.Salary < 0)
            {
                ModelState.AddModelError("Salary", "Salary cannot be negative.");
            }

            if (employee.HireDate > DateTime.Today)
            {
                ModelState.AddModelError("HireDate", "Hire date cannot be in the future.");
            }

            if (ModelState.IsValid)
            {
                var existingEmployee = await _context.Employees.FindAsync(id);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                if (!existingEmployee.IsActive)
                {
                    TempData["ErrorMessage"] = "Inactive employees cannot be edited.";
                    return RedirectToAction(nameof(Index));
                }

                existingEmployee.FirstName = employee.FirstName;
                existingEmployee.LastName = employee.LastName;
                existingEmployee.Email = employee.Email;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.DepartmentId = employee.DepartmentId;
                existingEmployee.Salary = employee.Salary;
                existingEmployee.HireDate = employee.HireDate;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            if (!employee.IsActive)
            {
                TempData["ErrorMessage"] = "This employee is already inactive.";
                return RedirectToAction(nameof(Index));
            }

            employee.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employee was deactivated successfully.";
            return RedirectToAction(nameof(Index));
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        public async Task<IActionResult> UpdateSalary(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            if (!employee.IsActive)
            {
                TempData["ErrorMessage"] = "Inactive employees cannot receive salary updates.";
                return RedirectToAction(nameof(Index));
            }

            var model = new UpdateSalaryViewModel
            {
                EmployeeId = employee.Id,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                CurrentSalary = employee.Salary,
                NewSalary = employee.Salary
            };

            return View(model);
        }
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSalary(UpdateSalaryViewModel model)
        {
            ModelState.Remove(nameof(model.CurrentSalary));
            ModelState.Remove(nameof(model.EmployeeName));

            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == model.EmployeeId);

            if (employee == null)
            {
                return NotFound();
            }

            if (!employee.IsActive)
            {
                TempData["ErrorMessage"] = "Inactive employees cannot receive salary updates.";
                return RedirectToAction(nameof(Index));
            }

            if (model.NewSalary < 0)
            {
                ModelState.AddModelError("NewSalary", "New salary cannot be negative.");
            }

            if (model.NewSalary == employee.Salary)
            {
                ModelState.AddModelError("NewSalary", "The new salary must be different from the current salary.");
            }

            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var salaryHistory = new EmployeeSalaryHistory
                    {
                        EmployeeId = employee.Id,
                        OldSalary = employee.Salary,
                        NewSalary = model.NewSalary,
                        ChangedAt = DateTime.UtcNow
                    };

                    _context.EmployeeSalaryHistories.Add(salaryHistory);

                    employee.Salary = model.NewSalary;

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    TempData["SuccessMessage"] = "Salary updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError(string.Empty, "An error occurred while updating the salary.");
                }
            }

            model.EmployeeName = $"{employee.FirstName} {employee.LastName}";
            model.CurrentSalary = employee.Salary;

            return View(model);
        }
        public async Task<IActionResult> SalaryHistory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.SalaryHistories)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.EmployeeName = $"{employee.FirstName} {employee.LastName}";

            var history = employee.SalaryHistories
                .OrderByDescending(h => h.ChangedAt)
                .ToList();

            return View(history);
        }


    }
}
