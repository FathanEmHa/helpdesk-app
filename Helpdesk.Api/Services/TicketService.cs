using Helpdesk.Data;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Exceptions;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Helpdesk.Helpers;
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
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Create(int userId, CreateTicketRequest request)
    {
        var user = await _context.Users
        	.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            throw new NotFoundException("User not found.");

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority!.Value,
            Status = TicketStatus.Open,
            UserId = userId,
            User = user
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Update(int id, UpdateTicketRequest request)
    {
        var ticket = await _context.Tickets
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();
        ticket.Priority = request.Priority!.Value;
        ticket.Status = request.Status!.Value;

        await _context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task Delete(int id)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        ticket.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}