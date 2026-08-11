using Helpdesk.Models.Base;

namespace Helpdesk.Models;

public class Ticket : SoftDeleteEntity
{
    public string TicketNumber { get; set; } = "";
    
    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public TicketPriority Priority { get; set; } = TicketPriority.Medium;

    public TicketStatus Status { get; set; } = TicketStatus.Open;

    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Comment> Comments { get; set; } = [];
}