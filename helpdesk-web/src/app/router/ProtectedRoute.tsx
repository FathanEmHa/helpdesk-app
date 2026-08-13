import { Navigate, Outlet } from "react-router";

function ProtectedRoute() {
  // Temporary auth state.
  // Will be replaced by AuthContext in Phase 3.
  const isAuthenticated = true;

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}

export default ProtectedRoute;