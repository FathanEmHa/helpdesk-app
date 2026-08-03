using Helpdesk.Data;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Dtos.Common;
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

    public async Task<PagedResponse<TicketListResponse>> GetAll(
        TicketQueryRequest request)
    {
        var currentUser = await _currentUserService.GetAsync();

        IQueryable<Ticket> query = _context.Tickets
            .Where(t => t.DeletedAt == null);

        if (currentUser.Role != Role.Admin)
        {
            query = query.Where(t => t.UserId == currentUser.Id);
        }

        var totalItems = await query.CountAsync();

        var tickets = await query
            .OrderByDescending(t => t.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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

        var totalPages = (int)Math.Ceiling(
            (double)totalItems / request.PageSize);

        return new PagedResponse<TicketListResponse>
        {
            Items = tickets,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<TicketDetailResponse> GetById(int id)
    {
        var ticket = await _context.Tickets
            .Include(t => t.User)
            .Include(t => t.Comments)
                .ThenInclude(c => c.User)
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Create(CreateMyTicketRequest request)
    {
        var currentUser = await _currentUserService.GetAsync();

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            
            UserId = currentUser.Id,
            User = currentUser,

            Priority = TicketPriority.Medium,
            Status = TicketStatus.Open,
        };

        _context.Tickets.Add(ticket);

        await _context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> UpdateMyTicket(
        int id,
        UpdateMyTicketRequest request)
    {
        var ticket = await _context.Tickets
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetAsync();

        if (ticket.UserId != currentUser.Id)
            throw new ForbiddenException("You cannot update user ticket own");

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();

        await _context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> UpdateTicketAdmin(
        int id,
        UpdateTicketAdminRequest request)
    {
        var ticket = await _context.Tickets
            .Include(t => t.User)
            .FirstOrDefaultAsync(t =>
                t.Id == id &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureAdmin(currentUser);

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

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        ticket.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}