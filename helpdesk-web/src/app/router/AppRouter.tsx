import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router";

import MainLayout from "../../components/layout/MainLayout";

import LoginPage from "../../features/auth/pages/LoginPage";
import DashboardPage from "../../features/dashboard/pages/DashboardPage";

import TicketsPage from "../../features/tickets/pages/TicketsPage";
import TicketDetailPage from "../../features/tickets/pages/TicketDetailPage";
import CreateTicketPage from "../../features/tickets/pages/CreateTicketPage";
import AdminTicketsPage from "../../features/tickets/pages/AdminTicketsPage";

import AdminUsersPage from "../../features/users/pages/AdminUsersPage";

import ProtectedRoute from "./ProtectedRoute";
import RoleRoute from "./RoleRoute";
import UnauthorizedPage from "./UnauthorizedPage";

function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public */}
        <Route
          path="/login"
          element={<LoginPage />}
        />

        <Route
          path="/unauthorized"
          element={<UnauthorizedPage />}
        />

        {/* Protected application */}
        <Route element={<ProtectedRoute />}>
          <Route element={<MainLayout />}>
            <Route
              path="/"
              element={
                <Navigate
                  to="/dashboard"
                  replace
                />
              }
            />

            <Route
              path="/dashboard"
              element={<DashboardPage />}
            />

            <Route
              path="/tickets"
              element={<TicketsPage />}
            />

            <Route
              path="/tickets/new"
              element={<CreateTicketPage />}
            />

            <Route
              path="/tickets/:id"
              element={<TicketDetailPage />}
            />

            {/* Admin */}
            <Route
              element={
                <RoleRoute
                  allowedRoles={["Admin"]}
                />
              }
            >
              <Route
                path="/admin/tickets"
                element={<AdminTicketsPage />}
              />

              <Route
                path="/admin/users"
                element={<AdminUsersPage />}
              />
            </Route>
          </Route>
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default AppRouter;