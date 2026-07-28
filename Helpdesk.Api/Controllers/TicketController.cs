using Helpdesk.Services;
using Helpdesk.Dtos.Ticket;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
	public async Task<IActionResult> GetAll()
	{
		var tickets = await _ticketService.GetAll();

		return Ok(tickets);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> GetById(int id)
	{
		var ticket = await _ticketService.GetById(id);

		return Ok(ticket);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateTicketRequest request)
	{
		var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

		var ticket = await _ticketService.Create(userId, request);

		return CreatedAtAction(
	        nameof(GetById),
	        new { id = ticket.Id },
	        ticket
	    );
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> Update(int id, UpdateTicketRequest request)
	{
		var ticket = await _ticketService.Update(id, request);

		return Ok(ticket);
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(int id)
	{
		await _ticketService.Delete(id);

		return NoContent();
	}
}

