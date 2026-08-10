using System.Linq.Expressions;
using Helpdesk.Dtos.Comment;
using Helpdesk.Models;

namespace Helpdesk.Mappers;

public static class CommentMapper
{
    public static CommentResponse ToCommentResponse(Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            TicketId = comment.TicketId,
            UserId = comment.UserId,
            UserName = comment.User.Name,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Version = comment.Version
        };
    }

    public static Expression<Func<Comment, CommentResponse>>
        ToCommentResponseProjection =>
        comment => new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            TicketId = comment.TicketId,
            UserId = comment.UserId,
            UserName = comment.User.Name,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            Version = comment.Version
        };
}