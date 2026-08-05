using Helpdesk.Dtos.Ticket;
using Helpdesk.Models;

namespace Helpdesk.Mappers;

public static class TicketMapper
{
    public static TicketDetailResponse ToDetailResponse(Ticket ticket)
    {
        return new TicketDetailResponse
        {
            Id = ticket.Id,
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
}