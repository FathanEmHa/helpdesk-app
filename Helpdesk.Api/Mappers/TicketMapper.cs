using System.Linq.Expressions;
using Helpdesk.Dtos.Ticket;
using Helpdesk.Dtos.Comment;
using Helpdesk.Models;

namespace Helpdesk.Mappers;

public static class TicketMapper
{
    public static TicketDetailResponse ToDetailResponse(Ticket ticket)
    {
        return new TicketDetailResponse
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority.ToString(),
            Status = ticket.Status.ToString(),
            UserId = ticket.UserId,
            UserName = ticket.User.Name,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            Version = ticket.Version,

            Comments = ticket.Comments
                .Where(c => c.DeletedAt == null)
                .OrderBy(c => c.CreatedAt)
                .Select(CommentMapper.ToCommentResponse)
                .ToList()
        };
    }

    public static Expression<Func<Ticket, TicketListResponse>>
        ToListResponseProjection =>
        ticket => new TicketListResponse
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Status = ticket.Status.ToString(),
            Priority = ticket.Priority.ToString(),
            UserId = ticket.UserId,
            UserName = ticket.User.Name,
            CommentCount = ticket.Comments.Count()
        };

    public static Expression<Func<Ticket, TicketDetailResponse>>
        ToDetailResponseProjection =>
        ticket => new TicketDetailResponse
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Title = ticket.Title,
            Description = ticket.Description,
            Priority = ticket.Priority.ToString(),
            Status = ticket.Status.ToString(),
            UserId = ticket.UserId,
            UserName = ticket.User.Name,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            Version = ticket.Version,

            Comments = ticket.Comments
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
                .ToList()
        };
}