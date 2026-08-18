import { Navigate, Outlet } from "react-router";
import { useAuth } from "../../features/auth/hooks/useAuth";

interface RoleRouteProps {
  allowedRoles: string[];
}

function RoleRoute({
  allowedRoles,
}: RoleRouteProps) {
  const { auth } = useAuth();

  const userRole = auth?.role;

  if (!userRole || !allowedRoles.includes(userRole)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <Outlet />;
}

export default RoleRoute;