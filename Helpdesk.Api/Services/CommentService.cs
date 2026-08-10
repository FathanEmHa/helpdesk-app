using Helpdesk.Data;
using Helpdesk.Exceptions;
using Helpdesk.Helpers;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Helpdesk.Services.Base;
using Helpdesk.Dtos.Comment;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class CommentService : BaseService
{
    public CommentService(
        AppDbContext context,
        CurrentUserService currentUserService)
        : base(context, currentUserService)
    {
    }

    private async Task<Ticket> GetTicketOrThrow(
        int id,
        bool tracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Ticket> query = tracking
            ? Context.Tickets
            : Context.Tickets.AsNoTracking();

        var ticket = await query.FirstOrDefaultAsync(
            t => t.Id == id,
            cancellationToken);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return ticket;
    }

    private async Task<Comment> GetCommentOrThrow(
        int id,
        bool tracking = true,
        bool includeUser = false,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Comment> query = tracking
            ? Context.Comments
            : Context.Comments.AsNoTracking();

        if (includeUser)
        {
            query = query.Include(c => c.User);
        }

        var comment = await query.FirstOrDefaultAsync(
            c => c.Id == id,
            cancellationToken);

        if (comment == null)
            throw new NotFoundException("Comment not found.");

        return comment;
    }

    // =========================
    // Read
    // =========================

    public async Task<List<CommentResponse>> GetByTicketId(
        int ticketId,
        CancellationToken cancellationToken)
    {
        var ticket = await GetTicketOrThrow(
            ticketId,
            tracking: false,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return await Context.Comments
            .AsNoTracking()
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedAt)
            .Select(CommentMapper.ToCommentResponseProjection)
            .ToListAsync(cancellationToken);
    }

    // =========================
    // Create
    // =========================

    public async Task<CommentResponse> Create(
        int ticketId,
        CreateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var ticket = await GetTicketOrThrow(
            ticketId,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        var comment = new Comment
        {
            Content = request.Content.Trim(),
            TicketId = ticketId,
            UserId = currentUser.Id,
            User = currentUser
        };

        Context.Comments.Add(comment);

        await Context.SaveChangesAsync(
            cancellationToken);

        return CommentMapper.ToCommentResponse(comment);
    }

    // =========================
    // Update
    // =========================

    public async Task<CommentResponse> Update(
        int id,
        UpdateCommentRequest request,
        CancellationToken cancellationToken)
    {
        var comment = await GetCommentOrThrow(
            id,
            includeUser: true,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        Context.Entry(comment)
            .Property(c => c.Version)
            .OriginalValue = request.Version;

        comment.Content = request.Content.Trim();

        await Context.SaveChangesAsync(
            cancellationToken);

        return CommentMapper.ToCommentResponse(comment);
    }

    // =========================
    // Delete
    // =========================

    public async Task Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var comment = await GetCommentOrThrow(
            id,
            cancellationToken: cancellationToken);

        var currentUser = await GetCurrentUser(
            cancellationToken);

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        comment.DeletedAt = DateTime.UtcNow;
        comment.DeletedBy = CurrentUserId;

        await Context.SaveChangesAsync(
            cancellationToken);
    }
}