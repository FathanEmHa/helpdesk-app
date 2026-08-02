using Helpdesk.Models;
using System.ComponentModel.DataAnnotations;

namespace Helpdesk.Dtos.Ticket;

public class UpdateTicketAdminRequest
{
    [Required(ErrorMessage = "Priority is required.")]
    public TicketPriority? Priority { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public TicketStatus? Status { get; set; }
}