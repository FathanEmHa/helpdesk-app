import { Search } from "lucide-react";
import AdminTicketTable from "../components/AdminTicketTable";

const tickets = [
  {
    id: "TCK-001",
    title: "Cannot access email",
    requester: "John Doe",
    status: "Open",
    priority: "High",
    createdAt: "Aug 13, 2026",
  },
  {
    id: "TCK-002",
    title: "Printer problem",
    requester: "Jane Smith",
    status: "Resolved",
    priority: "Medium",
    createdAt: "Aug 12, 2026",
  },
  {
    id: "TCK-003",
    title: "VPN connection issue",
    requester: "Michael Lee",
    status: "Pending",
    priority: "Low",
    createdAt: "Aug 12, 2026",
  },
  {
    id: "TCK-004",
    title: "Unable to access internal system",
    requester: "Sarah Wilson",
    status: "Open",
    priority: "High",
    createdAt: "Aug 11, 2026",
  },
];

function AdminTicketsPage() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">
          Manage Tickets
        </h1>

        <p className="mt-1 text-sm text-muted-foreground">
          View and manage all support tickets.
        </p>
      </div>

      {/* Filters */}
      <div className="flex flex-col gap-3 sm:flex-row">
        <div className="relative flex-1">
          <Search
            size={18}
            className="absolute left-3 top-1/2 -translate-y-1/2 text-muted-foreground"
          />

          <input
            type="search"
            placeholder="Search tickets..."
            className="w-full rounded-md border bg-background py-2.5 pl-10 pr-3 text-sm outline-none focus:ring-2 focus:ring-ring"
          />
        </div>

        <select
          defaultValue="all"
          className="rounded-md border bg-background px-3 py-2.5 text-sm outline-none focus:ring-2 focus:ring-ring"
        >
          <option value="all">All statuses</option>
          <option value="pending">Pending</option>
          <option value="open">Open</option>
          <option value="in-progress">In Progress</option>
          <option value="resolved">Resolved</option>
          <option value="closed">Closed</option>
        </select>

        <select
          defaultValue="all"
          className="rounded-md border bg-background px-3 py-2.5 text-sm outline-none focus:ring-2 focus:ring-ring"
        >
          <option value="all">All priorities</option>
          <option value="low">Low</option>
          <option value="medium">Medium</option>
          <option value="high">High</option>
        </select>
      </div>

      {/* Table */}
      <AdminTicketTable tickets={tickets} />

      {/* Pagination */}
      <div className="flex items-center justify-between">
        <p className="text-sm text-muted-foreground">
          Showing 1–4 of 4 tickets
        </p>

        <div className="flex items-center gap-1">
          <button
            type="button"
            disabled
            className="rounded-md border px-3 py-2 text-sm disabled:cursor-not-allowed disabled:opacity-50"
          >
            Previous
          </button>

          <button
            type="button"
            className="rounded-md bg-primary px-3 py-2 text-sm text-primary-foreground"
          >
            1
          </button>

          <button
            type="button"
            className="rounded-md border px-3 py-2 text-sm"
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
}

export default AdminTicketsPage;