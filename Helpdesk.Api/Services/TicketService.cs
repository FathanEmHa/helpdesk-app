using Helpdesk.Data;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Exceptions;
using Helpdesk.Helpers;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class TicketService
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public TicketService(
        AppDbContext context,
        CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<TicketListResponse>> GetAll()
    {
        var currentUser = await _currentUserService.GetCurrentUser();

        if (currentUser.Role == UserRole.Admin)
        {
            return await _context.Tickets
                .Where(t => t.DeletedAt == null)
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

        return await _context.Tickets
            .Where(t =>
                t.UserId == currentUser.Id &&
                t.DeletedAt == null)
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
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Create(CreateTicketRequest request)
    {
        var currentUser = await _currentUserService.GetCurrentUser();

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority!.Value,
            Status = TicketStatus.Open,
            UserId = currentUser.Id,
            User = currentUser
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Update(
        int id,
        UpdateTicketRequest request)
    {
        var ticket = await _context.Tickets
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

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
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        ticket.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}