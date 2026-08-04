using Helpdesk.Models;
using Helpdesk.Dtos.Common;

namespace Helpdesk.Dtos.Ticket;

public class TicketQueryRequest : PaginationRequest
{
	public string? Search { get; set; }

	public TicketStatus? Status { get; set; }

    public TicketPriority? Priority { get; set; }
}