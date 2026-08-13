interface TicketDetailProps {
  id: string;
  title: string;
  description: string;
  status: string;
  priority: string;
  createdAt: string;
}

function TicketDetail({
  id,
  title,
  description,
  status,
  priority,
  createdAt,
}: TicketDetailProps) {
  return (
    <div className="rounded-xl border bg-card">
      <div className="border-b p-6">
        <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
          <div>
            <p className="text-sm text-muted-foreground">
              #{id}
            </p>

            <h1 className="mt-1 text-2xl font-semibold tracking-tight">
              {title}
            </h1>
          </div>

          <div className="flex items-center gap-2">
            <span className="rounded-full bg-muted px-3 py-1 text-xs font-medium">
              {status}
            </span>

            <span className="rounded-full border px-3 py-1 text-xs font-medium">
              {priority}
            </span>
          </div>
        </div>
      </div>

      <div className="space-y-6 p-6">
        <div>
          <h2 className="text-sm font-medium">
            Description
          </h2>

          <p className="mt-2 whitespace-pre-wrap text-sm leading-6 text-muted-foreground">
            {description}
          </p>
        </div>

        <div>
          <h2 className="text-sm font-medium">
            Created
          </h2>

          <p className="mt-2 text-sm text-muted-foreground">
            {createdAt}
          </p>
        </div>
      </div>
    </div>
  );
}

export default TicketDetail;