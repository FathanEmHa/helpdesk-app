import { Bell, CircleUserRound, LogOut } from "lucide-react";
import { useAuth } from "../../features/auth/hooks/useAuth";

function Navbar() {
  const { logout, auth } = useAuth();

  return (
    <header className="flex h-16 items-center justify-between border-b bg-background px-6">
      {/* Brand */}
      <div className="flex items-center gap-2">
        <span className="text-lg font-semibold">
          Helpdesk
        </span>
      </div>

      {/* Right side */}
      <div className="flex items-center gap-4">
        <button
          type="button"
          className="rounded-md p-2 hover:bg-muted"
          aria-label="Notifications"
        >
          <Bell size={20} />
        </button>

        <div className="flex items-center gap-3">
          <div className="hidden text-right sm:block">
            <p className="text-sm font-medium">
              {auth?.name}
            </p>

            <p className="text-xs text-muted-foreground">
              {auth?.role}
            </p>
          </div>

          <button
            type="button"
            className="rounded-md p-2 hover:bg-muted"
            aria-label="User profile"
          >
            <CircleUserRound size={22} />
          </button>

          <button
            type="button"
            onClick={logout}
            className="rounded-md p-2 text-muted-foreground hover:bg-muted hover:text-foreground"
            aria-label="Logout"
            title="Logout"
          >
            <LogOut size={20} />
          </button>
        </div>
      </div>
    </header>
  );
}

export default Navbar;