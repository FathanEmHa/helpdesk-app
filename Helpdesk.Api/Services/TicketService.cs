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
    private readonly ActivityLogService _activityLogService;

    public TicketService(
        AppDbContext context,
        CurrentUserService currentUserService,
        ActivityLogService activityLogService)
        : base(context, currentUserService)
    {
        _activityLogService = activityLogService;
    }

    private async Task<Ticket> GetTicketOrThrow(
        int id,
        bool tracking = true,
        bool includeUser = false,
        bool includeComments = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Ticket> query = tracking
            ? Context.Tickets
            : Context.Tickets.AsNoTracking();

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

        var ticket = await query.FirstOrDefaultAsync(
            t => t.Id == id,
            cancellationToken);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return ticket;
    }

    public async Task<PagedResponse<TicketListResponse>> GetAll(
        TicketQueryRequest request,
        CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser(
            cancellationToken);

        IQueryable<Ticket> query =
            Context.Tickets.AsNoTracking();

        if (currentUser.Role != Role.Admin)
        {
            query = query.Where(t =>
                t.UserId == currentUser.Id);
        }

        query = query
            .ApplyFilters(request)
            .ApplySorting(request);

        var totalItems = await query.CountAsync(
            cancellationToken);

        var tickets = await query
            .ApplyPagination(
                request.Page,
                request.PageSize)
            .Select(TicketMapper.ToListResponseProjection)
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(
            (double)totalItems /
            request.PageSize);

        return new PagedResponse<TicketListResponse>
        {
            Items = tickets,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };
    }

    public async Task<TicketDetailResponse> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var currentUser = await GetCurrentUser(
            cancellationToken);

        var ticket = await Context.Tickets
            .AsNoTracking()
            .Where(t => t.Id == id)
            .Select(TicketMapper.ToDetailResponseProjection)
            .FirstOrDefaultAsync(cancellationToken);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return ticket;
    }

    public async Task<TicketDetailResponse> Create(
        CreateMyTicketRequest request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await Context.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            var ticket = new Ticket
            {
                Title = request.Title.Trim(),
                Description = request.Description.Trim(),
                UserId = CurrentUserId!.Value,
                Priority = TicketPriority.Medium,
                Status = TicketStatus.Open
            };

            Context.Tickets.Add(ticket);

            // Save pertama untuk mendapatkan Id.
            await Context.SaveChangesAsync(
                cancellationToken);

            ticket.TicketNumber =
                $"TKT-{DateTime.UtcNow:yyyy}-{ticket.Id:D6}";

            _activityLogService.Add(
                action: "Create",
                entityType: "Ticket",
                entityId: ticket.Id,
                description: $"Created ticket {ticket.TicketNumber}");

            await Context.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);

            return await GetById(
                ticket.Id,
                cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }
    }

    public async Task<TicketDetailResponse> UpdateMyTicket(
        int id,
        UpdateMyTicketRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await GetTicketOrThrow(
            id,
            includeUser: true,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        Context.Entry(ticket)
            .Property(t => t.Version)
            .OriginalValue = request.Version;

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description.Trim();

        _activityLogService.Add(
            action: "Update",
            entityType: "Ticket",
            entityId: ticket.Id,
            description: $"Updated ticket {ticket.TicketNumber}");

        await Context.SaveChangesAsync(
            cancellationToken);

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task<TicketDetailResponse> UpdateTicketAdmin(
        int id,
        UpdateTicketAdminRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await GetTicketOrThrow(
            id,
            includeUser: true,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureAdmin(
            currentUser);

        Context.Entry(ticket)
            .Property(t => t.Version)
            .OriginalValue = request.Version;

        ticket.Priority = request.Priority!.Value;
        ticket.Status = request.Status!.Value;

        _activityLogService.Add(
            action: "Update",
            entityType: "Ticket",
            entityId: ticket.Id,
            description: $"Admin updated ticket {ticket.TicketNumber}");

        await Context.SaveChangesAsync(
            cancellationToken);

        return TicketMapper.ToDetailResponse(ticket);
    }

    public async Task Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var ticket = await GetTicketOrThrow(
            id,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        ticket.DeletedAt = DateTime.UtcNow;
        ticket.DeletedBy = CurrentUserId;

        _activityLogService.Add(
            action: "Delete",
            entityType: "Ticket",
            entityId: ticket.Id,
            description: $"Deleted ticket {ticket.TicketNumber}");

        await Context.SaveChangesAsync(
            cancellationToken);
    }
}