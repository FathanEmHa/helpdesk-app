using System.ComponentModel.DataAnnotations;
using Helpdesk.Models;

namespace Helpdesk.Dtos.Ticket;

public class UpdateTicketRequest
{
    [Required]
    public string Title { get; set; } = "";

    [Required]
    public string Description { get; set; } = "";

    public TicketPriority Priority { get; set; }

    public TicketStatus Status { get; set; }
}