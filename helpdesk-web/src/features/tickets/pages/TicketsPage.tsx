import { useEffect, useState } from "react";
import { getTickets } from "../api/ticketApi";
import type { TicketListResponse } from "../types";
import { ApiError } from "../../../lib/apiError";
import { Link } from "react-router";
import { Plus, Search } from "lucide-react";
import TicketTable from "../components/TicketTable";

function TicketsPage() {
  const [tickets, setTickets] = useState<
    TicketListResponse[]
  >([]);

  const [isLoading, setIsLoading] =
    useState(true);

  const [error, setError] =
    useState<string | null>(null);

  useEffect(() => {
    async function fetchTickets() {
      setIsLoading(true);
      setError(null);

      try {
        const response = await getTickets();

        setTickets(response.items);
      } catch (error) {
        if (error instanceof ApiError) {
          setError(error.message);
        } else {
          setError("Failed to load tickets.");
        }
      } finally {
        setIsLoading(false);
      }
    }

    fetchTickets();
  }, []);

  if (isLoading) {
    return (
      <div className="p-6">
        <p className="text-sm text-muted-foreground">
          Loading tickets...
        </p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-6">
        <p className="text-sm text-destructive">
          {error}
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">
            Tickets
          </h1>

          <p className="mt-1 text-sm text-muted-foreground">
            Manage your support requests.
          </p>
        </div>

        <Link
          to="/tickets/new"
          className="inline-flex items-center justify-center gap-2 rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground hover:opacity-90"
        >
          <Plus size={18} />
          New Ticket
        </Link>
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
      <TicketTable tickets={tickets} />

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

export default TicketsPage;