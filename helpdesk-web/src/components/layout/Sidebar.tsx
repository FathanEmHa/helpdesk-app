import {
  LayoutDashboard,
  Ticket,
  Plus,
  Users,
  ShieldCheck,
} from "lucide-react";
import { NavLink } from "react-router";
import { useAuth } from "../../features/auth/hooks/useAuth";

function Sidebar() {
  const { auth } = useAuth();

  const isAdmin = auth?.role === "Admin";

  return (
    <aside className="w-64 shrink-0 border-r bg-background">
      <nav className="space-y-1 p-4">
        <NavLink
          to="/dashboard"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm hover:bg-muted"
        >
          <LayoutDashboard size={18} />
          Dashboard
        </NavLink>

        <NavLink
          to="/tickets"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm hover:bg-muted"
        >
          <Ticket size={18} />
          Tickets
        </NavLink>

        <NavLink
          to="/tickets/new"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm hover:bg-muted"
        >
          <Plus size={18} />
          Create Ticket
        </NavLink>

        {isAdmin && (
          <>
            <div className="pt-4 pb-2 px-3 text-xs font-medium text-muted-foreground">
              Administration
            </div>

            <NavLink
              to="/admin/tickets"
              className="flex items-center gap-3 rounded-md px-3 py-2 text-sm hover:bg-muted"
            >
              <ShieldCheck size={18} />
              Manage Tickets
            </NavLink>

            <NavLink
              to="/admin/users"
              className="flex items-center gap-3 rounded-md px-3 py-2 text-sm hover:bg-muted"
            >
              <Users size={18} />
              Manage Users
            </NavLink>
          </>
        )}
      </nav>
    </aside>
  );
}

export default Sidebar;