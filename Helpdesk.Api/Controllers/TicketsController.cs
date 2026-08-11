using Helpdesk.Dtos.Ticket;
using Helpdesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] TicketQueryRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _ticketService.GetAll(
            request,
            cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        return Ok(await _ticketService.GetById(
            id,
            cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMyTicketRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.Create(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = ticket.Id },
            ticket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateMyTicketRequest request,
        CancellationToken cancellationToken)
    {
        return Ok(await _ticketService.UpdateMyTicket(
            id,
            request,
            cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _ticketService.Delete(
            id,
            cancellationToken);

        return NoContent();
    }
}