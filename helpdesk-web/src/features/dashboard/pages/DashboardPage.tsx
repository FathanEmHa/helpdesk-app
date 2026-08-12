import StatCard from "../components/StatCard";

const recentTickets = [
  {
    id: "TCK-001",
    title: "Cannot access email",
    status: "Open",
    priority: "High",
  },
  {
    id: "TCK-002",
    title: "Printer problem",
    status: "Resolved",
    priority: "Medium",
  },
  {
    id: "TCK-003",
    title: "VPN connection issue",
    status: "Open",
    priority: "Low",
  },
];

function DashboardPage() {
  return (
    <div className="space-y-8">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">
          Dashboard
        </h1>

        <p className="mt-1 text-sm text-muted-foreground">
          Welcome back. Here's what's happening with your
          tickets.
        </p>
      </div>

      {/* Statistics */}
      <section className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
        <StatCard
          title="Total Tickets"
          value={12}
          description="All your tickets"
        />

        <StatCard
          title="Open Tickets"
          value={4}
          description="Currently unresolved"
        />

        <StatCard
          title="Resolved Tickets"
          value={8}
          description="Successfully resolved"
        />
      </section>

      {/* Recent Tickets */}
      <section className="space-y-4">
        <div>
          <h2 className="text-lg font-semibold">
            Recent Tickets
          </h2>

          <p className="text-sm text-muted-foreground">
            Your most recent support requests.
          </p>
        </div>

        <div className="overflow-hidden rounded-xl border">
          {recentTickets.map((ticket) => (
            <div
              key={ticket.id}
              className="flex items-center justify-between gap-4 border-b p-4 last:border-b-0"
            >
              <div className="min-w-0">
                <p className="text-sm font-medium">
                  {ticket.title}
                </p>

                <p className="mt-1 text-xs text-muted-foreground">
                  #{ticket.id}
                </p>
              </div>

              <div className="flex shrink-0 items-center gap-3">
                <span className="text-xs text-muted-foreground">
                  {ticket.priority}
                </span>

                <span className="rounded-full bg-muted px-2.5 py-1 text-xs font-medium">
                  {ticket.status}
                </span>
              </div>
            </div>
          ))}
        </div>
      </section>
    </div>
  );
}

export default DashboardPage;