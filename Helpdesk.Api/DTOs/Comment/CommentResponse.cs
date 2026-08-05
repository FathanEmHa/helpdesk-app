namespace Helpdesk.Dtos.Comment;

public class CommentResponse
{
    public int Id { get; set; }

    public string Content { get; set; } = "";

    public int TicketId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public uint Version { get; set; }
}