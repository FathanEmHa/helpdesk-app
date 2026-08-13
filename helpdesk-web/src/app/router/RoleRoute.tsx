import { Navigate, Outlet } from "react-router";

interface RoleRouteProps {
  allowedRoles: string[];
}

function RoleRoute({
  allowedRoles,
}: RoleRouteProps) {
  // Temporary role.
  // Will be replaced by AuthContext in Phase 3.
  const userRole = "User";

  if (!allowedRoles.includes(userRole)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <Outlet />;
}

export default RoleRoute;