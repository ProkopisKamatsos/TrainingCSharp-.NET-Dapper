using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Repositories;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository _comments;

    public CommentsController(ICommentRepository comments)
    {
        _comments = comments;
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _comments.DeleteAsync(id);
        if (!ok) return NotFound();

        return NoContent();
    }
}
