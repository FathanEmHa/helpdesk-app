interface User {
  id: string;
  name: string;
  email: string;
  role: string;
  status: string;
  createdAt: string;
}

interface UserTableProps {
  users: User[];
}

function UserTable({ users }: UserTableProps) {
  return (
    <div className="overflow-hidden rounded-xl border">
      <div className="overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="border-b bg-muted/50">
            <tr>
              <th className="px-4 py-3 text-left font-medium">
                User
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Email
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Role
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Status
              </th>

              <th className="px-4 py-3 text-left font-medium">
                Created
              </th>
            </tr>
          </thead>

          <tbody>
            {users.map((user) => (
              <tr
                key={user.id}
                className="border-b last:border-b-0 hover:bg-muted/30"
              >
                <td className="px-4 py-3 font-medium">
                  {user.name}
                </td>

                <td className="px-4 py-3 text-muted-foreground">
                  {user.email}
                </td>

                <td className="px-4 py-3">
                  <span className="rounded-full bg-muted px-2.5 py-1 text-xs font-medium">
                    {user.role}
                  </span>
                </td>

                <td className="px-4 py-3">
                  <span className="rounded-full bg-muted px-2.5 py-1 text-xs font-medium">
                    {user.status}
                  </span>
                </td>

                <td className="whitespace-nowrap px-4 py-3 text-muted-foreground">
                  {user.createdAt}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default UserTable;