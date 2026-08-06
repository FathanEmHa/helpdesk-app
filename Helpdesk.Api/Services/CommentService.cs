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
        bool tracking = true)
    {
        IQueryable<Ticket> query = tracking
            ? Context.Tickets
            : Context.Tickets.AsNoTracking();

        var ticket = await query
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return ticket;
    }

    private async Task<Comment> GetCommentOrThrow(
        int id,
        bool tracking = true,
        bool includeUser = false)
    {
        IQueryable<Comment> query = tracking
            ? Context.Comments
            : Context.Comments.AsNoTracking();

        if (includeUser)
        {
            query = query.Include(c => c.User);
        }

        var comment = await query
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
            throw new NotFoundException("Comment not found.");

        return comment;
    }

    public async Task<List<CommentResponse>> GetByTicketId(int ticketId)
    {
        var ticket = await GetTicketOrThrow(
            ticketId,
            tracking: false);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return await Context.Comments
            .AsNoTracking()
            .Where(c => c.TicketId == ticketId)
            .OrderBy(c => c.CreatedAt)
            .Select(c => new CommentResponse
            {
                Id = c.Id,
                Content = c.Content,
                TicketId = c.TicketId,
                UserId = c.UserId,
                UserName = c.User.Name,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Version = c.Version
            })
            .ToListAsync();
    }

    public async Task<CommentResponse> Create(
        int ticketId,
        CreateCommentRequest request)
    {
        var ticket = await GetTicketOrThrow(ticketId);

        var currentUser = await GetCurrentUser();

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

        await Context.SaveChangesAsync();

        return CommentMapper.ToCommentResponse(comment);
    }

    public async Task<CommentResponse> Update(
        int id,
        UpdateCommentRequest request)
    {
        var comment = await GetCommentOrThrow(
            id,
            includeUser: true);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        Context.Entry(comment)
            .Property(c => c.Version)
            .OriginalValue = request.Version;

        comment.Content = request.Content.Trim();

        await Context.SaveChangesAsync();

        return CommentMapper.ToCommentResponse(comment);
    }

    public async Task Delete(int id)
    {
        var comment = await GetCommentOrThrow(id);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        comment.DeletedAt = DateTime.UtcNow;
        comment.DeletedBy = CurrentUserId;

        await Context.SaveChangesAsync();
    }
}