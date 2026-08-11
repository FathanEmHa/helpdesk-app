namespace Helpdesk.Dtos.Ticket;

public class TicketListResponse
{
    public int Id { get; set; }

    public string TicketNumber { get; set; } = "";

    public string Title { get; set; } = "";

    public string Status { get; set; } = "";

    public string Priority { get; set; } = "";

    public int UserId { get; set; }

    public string UserName { get; set; } = "";

    public int CommentCount { get; set; }

    public DateTime CreatedAt { get; set; }
}