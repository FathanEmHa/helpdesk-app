import { Bell, CircleUserRound } from "lucide-react";

function Navbar() {
  return (
    <header className="flex h-16 items-center justify-between border-b bg-background px-6">
      {/* Brand */}
      <div className="flex items-center gap-2">
        <span className="text-lg font-semibold">Helpdesk</span>
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

        <button
          type="button"
          className="rounded-md p-2 hover:bg-muted"
          aria-label="User profile"
        >
          <CircleUserRound size={22} />
        </button>
      </div>
    </header>
  );
}

export default Navbar;