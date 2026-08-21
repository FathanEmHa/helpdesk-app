import type { TicketListResponse } from "../types";

interface TicketTableProps {
  tickets: TicketListResponse[];
}

function TicketTable({
  tickets,
}: TicketTableProps) {
  if (tickets.length === 0) {
    return (
      <div className="rounded-lg border border-dashed p-8 text-center">
        <p className="text-sm text-muted-foreground">
          No tickets found.
        </p>
      </div>
    );
  }

  return (
    <div className="overflow-x-auto rounded-lg border">
      <table className="w-full text-sm">
        <thead className="border-b bg-muted/50">
          <tr>
            <th className="px-4 py-3 text-left font-medium">
              Ticket
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
              Comments
            </th>

            <th className="px-4 py-3 text-left font-medium">
              User
            </th>
          </tr>
        </thead>

        <tbody className="divide-y">
          {tickets.map((ticket) => (
            <tr
              key={ticket.id}
              className="transition hover:bg-muted/50"
            >
              <td className="px-4 py-3 font-medium">
                {ticket.ticketNumber}
              </td>

              <td className="px-4 py-3">
                {ticket.title}
              </td>

              <td className="px-4 py-3">
                {ticket.status}
              </td>

              <td className="px-4 py-3">
                {ticket.priority}
              </td>

              <td className="px-4 py-3">
                {ticket.commentCount}
              </td>

              <td className="px-4 py-3">
                {ticket.userName}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default TicketTable;