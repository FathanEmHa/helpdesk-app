using Helpdesk.Dtos.Comment;
using Helpdesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;

    public CommentsController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateCommentRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _commentService.Update(
            id,
            request,
            cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _commentService.Delete(
            id,
            cancellationToken);

        return NoContent();
    }
}