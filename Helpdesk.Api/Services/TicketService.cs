using Helpdesk.Data;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Models;
using Helpdesk.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class TicketService
{
	private readonly AppDbContext _context;

	public TicketService(AppDbContext context)
	{
		_context = context;
	}

	public async Task<List<TicketListResponse>> GetAll()
	{
		return await _context.Tickets
			.Select(t => new TicketListResponse
			{
				Id = t.Id,
		        Title = t.Title,
		        Status = t.Status.ToString(),
		        Priority = t.Priority.ToString(),

		        UserId = t.UserId,
		        UserName = t.User.Name
			})
			.ToListAsync();
	}

	public async Task<TicketDetailResponse> GetById(int id)
	{
	    var ticket = await _context.Tickets
	        .Where(t => t.Id == id)
	        .Select(t => new TicketDetailResponse
	        {
	            Id = t.Id,
	            Title = t.Title,
	            Description = t.Description,
	            Status = t.Status.ToString(),
	            Priority = t.Priority.ToString(),
	            UserId = t.UserId,
	            UserName = t.User.Name
	        })
	        .FirstOrDefaultAsync();

	    if (ticket == null)
	        throw new NotFoundException("Ticket not found.");

	    return ticket;
	}

	public async Task<TicketDetailResponse> Create(int userId, CreateTicketRequest request)
	{
	    var user = await _context.Users.FindAsync(userId);

	    if (user == null)
	        throw new NotFoundException("User not found.");

	    var ticket = new Models.Ticket
	    {
	        Title = request.Title.Trim(),
	        Description = request.Description.Trim(),
	        Priority = request.Priority!.Value,
	        Status = TicketStatus.Open,
	        UserId = userId
	    };

	    _context.Tickets.Add(ticket);
	    await _context.SaveChangesAsync();

	    return new TicketDetailResponse
	    {
	        Id = ticket.Id,
	        Title = ticket.Title,
	        Description = ticket.Description,
	       	Status = ticket.Status.ToString(),
	        Priority = ticket.Priority.ToString(),
	        UserId = user.Id,
	        UserName = user.Name
	    };
	}
}