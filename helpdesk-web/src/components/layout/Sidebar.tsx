import {
  LayoutDashboard,
  Ticket,
  Users,
} from "lucide-react";
import { NavLink } from "react-router";

function Sidebar() {
  return (
    <aside className="w-64 shrink-0 border-r bg-background">
      <nav className="flex flex-col gap-1 p-4">
        <NavLink
          to="/dashboard"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium hover:bg-muted"
        >
          <LayoutDashboard size={18} />
          Dashboard
        </NavLink>

        <NavLink
          to="/tickets"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium hover:bg-muted"
        >
          <Ticket size={18} />
          Tickets
        </NavLink>

        <NavLink
          to="/users"
          className="flex items-center gap-3 rounded-md px-3 py-2 text-sm font-medium hover:bg-muted"
        >
          <Users size={18} />
          Users
        </NavLink>
      </nav>
    </aside>
  );
}

export default Sidebar;