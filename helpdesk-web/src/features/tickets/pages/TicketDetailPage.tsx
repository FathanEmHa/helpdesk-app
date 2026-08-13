import { Link, useParams } from "react-router";
import { ArrowLeft } from "lucide-react";
import TicketDetail from "../components/TicketDetail";

function TicketDetailPage() {
  const { id } = useParams();

  const ticket = {
    id: id ?? "TCK-001",
    title: "Cannot access email",
    description:
      "I cannot access my company email account since this morning. The login page keeps showing an authentication error.",
    status: "Open",
    priority: "High",
    createdAt: "August 13, 2026",
  };

  return (
    <div className="space-y-6">
      <Link
        to="/tickets"
        className="inline-flex items-center gap-2 text-sm text-muted-foreground hover:text-foreground"
      >
        <ArrowLeft size={16} />
        Back to tickets
      </Link>

      <TicketDetail {...ticket} />
    </div>
  );
}

export default TicketDetailPage;