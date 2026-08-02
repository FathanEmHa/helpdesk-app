using Helpdesk.Dtos.Ticket;
using Helpdesk.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Helpdesk.Controllers;

[ApiController]
[Route("api/admin/tickets")]
public class AdminTicketsController : ControllerBase
{
    private readonly TicketService _ticketService;

    public AdminTicketsController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateTicketAdminRequest request)
    {
        return Ok(await _ticketService.UpdateTicketAdmin(id, request));
    }
}