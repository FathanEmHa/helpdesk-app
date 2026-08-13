import { Link } from "react-router";
import { ArrowLeft } from "lucide-react";
import TicketForm from "../components/TicketForm";

function CreateTicketPage() {
  return (
    <div className="mx-auto max-w-2xl space-y-6">
      <Link
        to="/tickets"
        className="inline-flex items-center gap-2 text-sm text-muted-foreground hover:text-foreground"
      >
        <ArrowLeft size={16} />
        Back to tickets
      </Link>

      <div>
        <h1 className="text-2xl font-semibold tracking-tight">
          Create Ticket
        </h1>

        <p className="mt-1 text-sm text-muted-foreground">
          Submit a new support request.
        </p>
      </div>

      <div className="rounded-xl border bg-card p-6">
        <TicketForm />
      </div>
    </div>
  );
}

export default CreateTicketPage;