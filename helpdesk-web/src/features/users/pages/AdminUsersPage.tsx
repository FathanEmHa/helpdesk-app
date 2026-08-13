import { Search } from "lucide-react";
import UserTable from "../components/UserTable";

const users = [
  {
    id: "USR-001",
    name: "John Doe",
    email: "john@example.com",
    role: "User",
    status: "Active",
    createdAt: "Aug 10, 2026",
  },
  {
    id: "USR-002",
    name: "Jane Smith",
    email: "jane@example.com",
    role: "User",
    status: "Active",
    createdAt: "Aug 9, 2026",
  },
  {
    id: "USR-003",
    name: "Admin",
    email: "admin@example.com",
    role: "Admin",
    status: "Active",
    createdAt: "Aug 1, 2026",
  },
  {
    id: "USR-004",
    name: "Michael Lee",
    email: "michael@example.com",
    role: "User",
    status: "Inactive",
    createdAt: "Jul 28, 2026",
  },
];

function AdminUsersPage() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">
          Manage Users
        </h1>

        <p className="mt-1 text-sm text-muted-foreground">
          View and manage users in the helpdesk system.
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
            placeholder="Search users..."
            className="w-full rounded-md border bg-background py-2.5 pl-10 pr-3 text-sm outline-none focus:ring-2 focus:ring-ring"
          />
        </div>

        <select
          defaultValue="all"
          className="rounded-md border bg-background px-3 py-2.5 text-sm outline-none focus:ring-2 focus:ring-ring"
        >
          <option value="all">All roles</option>
          <option value="user">User</option>
          <option value="admin">Admin</option>
        </select>

        <select
          defaultValue="all"
          className="rounded-md border bg-background px-3 py-2.5 text-sm outline-none focus:ring-2 focus:ring-ring"
        >
          <option value="all">All statuses</option>
          <option value="active">Active</option>
          <option value="inactive">Inactive</option>
        </select>
      </div>

      {/* Table */}
      <UserTable users={users} />

      {/* Pagination */}
      <div className="flex items-center justify-between">
        <p className="text-sm text-muted-foreground">
          Showing 1–4 of 4 users
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

export default AdminUsersPage;