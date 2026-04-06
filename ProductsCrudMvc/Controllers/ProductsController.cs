using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsCrudMvc.Data;
using ProductsCrudMvc.Models;

namespace ProductsCrudMvc.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Products
    [AllowAnonymous]
    public async Task<IActionResult> Index(string? search, string? category)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.Name.Contains(search) || (p.Description != null && p.Description.Contains(search)));

        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        ViewBag.Categories = await _context.Products
            .Select(p => p.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

        ViewBag.Search = search;
        ViewBag.SelectedCategory = category;

        return View(await query.OrderByDescending(p => p.CreatedAt).ToListAsync());
    }

    // GET: Products/Details/5
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        return View(product);
    }

    // GET: Products/Create
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Το προϊόν '{product.Name}' δημιουργήθηκε επιτυχώς!";
        return RedirectToAction(nameof(Index));
    }

    // GET: Products/Edit/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return NotFound();
        if (!ModelState.IsValid) return View(product);

        try
        {
            _context.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Το προϊόν '{product.Name}' ενημερώθηκε επιτυχώς!";
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Products.Any(p => p.Id == id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Products/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return NotFound();

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = $"Το προϊόν διαγράφηκε επιτυχώς!";
        }

        return RedirectToAction(nameof(Index));
    }
}
