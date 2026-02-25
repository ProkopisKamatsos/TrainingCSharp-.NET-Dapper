using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categories;

    public CategoriesController(ICategoryRepository categories)
    {
        _categories = categories;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _categories.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        dto.Name = dto.Name.Trim();
        var id = await _categories.CreateAsync(dto);

        return StatusCode(201, new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        dto.Name = dto.Name.Trim();
        var ok = await _categories.UpdateAsync(id, dto);

        if (!ok) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _categories.DeleteAsync(id);
        if (!ok) return NotFound();

        return NoContent();
    }
}
