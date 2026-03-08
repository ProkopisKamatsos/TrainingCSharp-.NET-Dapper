using EmployeeManagementSystemMVC.Data;
using EmployeeManagementSystemMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystemMVC.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Departments.Include(d => d.Manager);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Email");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location")] Department department)
        {
            bool departmentExists = await _context.Departments
            .AnyAsync(d => d.Name == department.Name);

            if (departmentExists)
            {
                ModelState.AddModelError("Name", "A department with this name already exists.");
            }
            if (ModelState.IsValid)
            {

                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Email", department.ManagerId);
            return View(department);
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Email", department.ManagerId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }
            bool departmentExists = await _context.Departments
            .AnyAsync(d => d.Name == department.Name && d.Id != department.Id);

            if (departmentExists)
            {
                ModelState.AddModelError("Name", "A department with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Employees, "Id", "Email", department.ManagerId);
            return View(department);
        }

        // GET: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Manager)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            if (department.Employees.Any())
            {
                TempData["ErrorMessage"] = "This department cannot be deleted because it has employees.";
                return RedirectToAction(nameof(Index));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Totals()
        {
            var departmentTotals = await _context.Departments
                .Select(d => new DepartmentTotalsViewModel
                {
                    DepartmentName = d.Name,
                    TotalEmployees = d.Employees.Count(),
                    ActiveEmployees = d.Employees.Count(e => e.IsActive),
                    TotalSalary = d.Employees.Where(e => e.IsActive).Sum(e => (decimal?)e.Salary) ?? 0,
                    AverageSalary = d.Employees.Where(e => e.IsActive).Average(e => (decimal?)e.Salary) ?? 0
                })
                .OrderBy(d => d.DepartmentName)
                .ToListAsync();

            return View(departmentTotals);
        }
    }

}
