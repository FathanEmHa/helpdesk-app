using Helpdesk.Services.Base;
using Helpdesk.Data;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Dtos.Common;
using Helpdesk.Exceptions;
using Helpdesk.Helpers;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Helpdesk.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class TicketService : BaseService
{
    public TicketService(
        AppDbContext context,
        CurrentUserService currentUserService)
        : base(context, currentUserService)
    {
    }
    
    private async Task<Ticket> GetTicketOrThrow(
        int id,
        bool includeUser = false,
        bool includeComments = false)
    {
        IQueryable<Ticket> query = Context.Tickets.AsQueryable();

        if (includeUser)
        {
            query = query.Include(t => t.User);
        }

        if (includeComments)
        {
            query = query
                .Include(t => t.Comments)
                .ThenInclude(c => c.User);
        }

        var ticket = await query.FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return ticket;
    }

    public async Task<PagedResponse<TicketListResponse>> GetAll(
        TicketQueryRequest request)
    {
        var currentUser = await GetCurrentUser();

        IQueryable<Ticket> query = Context.Tickets;

        if (currentUser.Role != Role.Admin)
        {
            query = query.Where(t => t.UserId == currentUser.Id);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var keyword = request.Search.Trim();

            query = query.Where(t =>
                EF.Functions.ILike(t.Title, $"%{keyword}%") ||
                EF.Functions.ILike(t.Description, $"%{keyword}%"));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(t =>
                t.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(t =>
                t.Priority == request.Priority.Value);
        }

        query = request.SortBy switch
        {
            TicketSortBy.Title => request.Descending
                ? query.OrderByDescending(t => t.Title)
                : query.OrderBy(t => t.Title),

            TicketSortBy.Priority => request.Descending
                ? query.OrderByDescending(t => t.Priority)
                : query.OrderBy(t => t.Priority),

            TicketSortBy.Status => request.Descending
                ? query.OrderByDescending(t => t.Status)
                : query.OrderBy(t => t.Status),

            TicketSortBy.CreatedAt => request.Descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt),

            _ => request.Descending
                ? query.OrderByDescending(t => t.CreatedAt)
                : query.OrderBy(t => t.CreatedAt)
        };

        var totalItems = await query.CountAsync();

        var tickets = await query
            .ApplyPagination(
                request.Page,
                request.PageSize)
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
        var ticket = await GetTicketOrThrow(
            id,
            includeUser: true,
            includeComments: true);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> Create(CreateMyTicketRequest request)
    {
        var currentUser = await GetCurrentUser();

        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            
            UserId = currentUser.Id,
            User = currentUser,

            Priority = TicketPriority.Medium,
            Status = TicketStatus.Open,
        };

        Context.Tickets.Add(ticket);

        await Context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> UpdateMyTicket(
        int id,
        UpdateMyTicketRequest request)
    {
        var ticket = await GetTicketOrThrow(
            id,
            includeUser: true);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();

        await Context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> UpdateTicketAdmin(
        int id,
        UpdateTicketAdminRequest request)
    {
        var ticket = await GetTicketOrThrow(
            id,
            includeUser: true);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureAdmin(currentUser);

        ticket.Priority = request.Priority!.Value;
        ticket.Status = request.Status!.Value;

        await Context.SaveChangesAsync();

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task Delete(int id)
    {
        var ticket = await GetTicketOrThrow(id);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        ticket.DeletedAt = DateTime.UtcNow;

        await Context.SaveChangesAsync();
    }
}