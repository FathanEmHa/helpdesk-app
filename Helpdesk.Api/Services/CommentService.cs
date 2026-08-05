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
    
    private async Task<Ticket> GetTicketOrThrow(int id)
    {
        var ticket = await Context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        return ticket;
    }

    private async Task<Comment> GetCommentOrThrow(
        int id,
        bool includeUser = false)
    {
        IQueryable<Comment> query = Context.Comments.AsQueryable();

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
        var ticket = await GetTicketOrThrow(ticketId);

        var currentUser = await GetCurrentUser();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return await Context.Comments
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
                UpdatedAt = c.UpdatedAt
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

        await Context.SaveChangesAsync();
    }
}