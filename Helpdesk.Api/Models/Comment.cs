using Helpdesk.Models.Base;

namespace Helpdesk.Models;

public class Comment : SoftDeleteEntity
{
    public string Content { get; set; } = "";

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}