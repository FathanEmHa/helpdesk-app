using Helpdesk.Data;
using Helpdesk.Dtos.Comment;
using Helpdesk.Exceptions;
using Helpdesk.Helpers;
using Helpdesk.Mappers;
using Helpdesk.Models;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Services;

public class CommentService
{
    private readonly AppDbContext _context;
    private readonly CurrentUserService _currentUserService;

    public CommentService(
        AppDbContext context,
        CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<CommentResponse>> GetByTicketId(int ticketId)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t =>
                t.Id == ticketId &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            ticket.UserId,
            currentUser);

        return await _context.Comments
            .Where(c =>
                c.TicketId == ticketId &&
                c.DeletedAt == null)
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
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t =>
                t.Id == ticketId &&
                t.DeletedAt == null);

        if (ticket == null)
            throw new NotFoundException("Ticket not found.");

        var currentUser = await _currentUserService.GetAsync();

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

        _context.Comments.Add(comment);

        await _context.SaveChangesAsync();

        return CommentMapper.ToCommentResponse(comment);
    }

    public async Task<CommentResponse> Update(
        int id,
        UpdateCommentRequest request)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.DeletedAt == null);

        if (comment == null)
            throw new NotFoundException("Comment not found.");

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        comment.Content = request.Content.Trim();
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return CommentMapper.ToCommentResponse(comment);
    }

    public async Task Delete(int id)
    {
        var comment = await _context.Comments
            .FirstOrDefaultAsync(c =>
                c.Id == id &&
                c.DeletedAt == null);

        if (comment == null)
            throw new NotFoundException("Comment not found.");

        var currentUser = await _currentUserService.GetAsync();

        AuthorizationHelper.EnsureOwnerOrAdmin(
            comment.UserId,
            currentUser);

        comment.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }
}