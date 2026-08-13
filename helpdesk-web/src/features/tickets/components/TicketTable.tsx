import { Link } from "react-router";

interface Ticket {
  id: string;
  title: string;
  status: string;
  priority: string;
}

interface TicketTableProps {
  tickets: Ticket[];
}

function TicketTable({ tickets }: TicketTableProps) {
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
                Title
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Status
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Priority
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
                  <Link
                    to={`/tickets/${ticket.id}`}
                    className="hover:underline"
                  >
                    #{ticket.id}
                  </Link>
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
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default TicketTable;