interface AdminTicket {
  id: string;
  title: string;
  requester: string;
  status: string;
  priority: string;
  createdAt: string;
}

interface AdminTicketTableProps {
  tickets: AdminTicket[];
}

function AdminTicketTable({
  tickets,
}: AdminTicketTableProps) {
  return (
    <div className="overflow-hidden rounded-xl border">
      <div className="overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="border-b bg-muted/50">
            <tr>
              <th className="px-4 py-3 text-left font-medium">
                Ticket
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Requester
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Title
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Status
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Priority
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Created
              </th>
            </tr>
          </thead>

          <tbody>
            {tickets.map((ticket) => (
              <tr
                key={ticket.id}
                className="border-b last:border-b-0 hover:bg-muted/30"
              >
                <td className="whitespace-nowrap px-4 py-3 font-medium">
                  #{ticket.id}
                </td>

                <td className="px-4 py-3">
                  {ticket.requester}
                </td>

                <td className="px-4 py-3">
                  {ticket.title}
                </td>

                <td className="px-4 py-3">
                  <span className="rounded-full bg-muted px-2.5 py-1 text-xs font-medium">
                    {ticket.status}
                  </span>
                </td>

                <td className="px-4 py-3 text-muted-foreground">
                  {ticket.priority}
                </td>

                <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                  {ticket.createdAt}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default AdminTicketTable;