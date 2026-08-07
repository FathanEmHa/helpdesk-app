using Helpdesk.Dtos.Comment;
using Helpdesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/tickets")]
[Authorize]
public class TicketCommentController : ControllerBase
{
    private readonly CommentService _commentService;

    public TicketCommentController(CommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("{ticketId}/comments")]
    public async Task<IActionResult> GetByTicketId(
        int ticketId,
        CancellationToken cancellationToken)
    {
        return Ok(await _commentService.GetByTicketId(
            ticketId,
            cancellationToken));
    }

    [HttpPost("{ticketId}/comments")]
    public async Task<IActionResult> Create(
        int ticketId,
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await _commentService.Create(
            ticketId,
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetByTicketId),
            new { ticketId },
            comment);
    }
}