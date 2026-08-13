import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import MainLayout from "./components/layout/MainLayout";
import LoginPage from "./features/auth/pages/LoginPage";
import DashboardPage from "./features/dashboard/pages/DashboardPage";

import TicketsPage from "./features/tickets/pages/TicketsPage";
import TicketDetailPage from "./features/tickets/pages/TicketDetailPage";
import CreateTicketPage from "./features/tickets/pages/CreateTicketPage";

import AdminTicketsPage from "./features/tickets/pages/AdminTicketsPage";
import AdminUsersPage from "./features/users/pages/AdminUsersPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />

        <Route element={<MainLayout />}>
          <Route
            path="/"
            element={<Navigate to="/login" replace />}
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

          <Route
            path="/admin/tickets"
            element={<AdminTicketsPage />}
          />

          <Route
            path="/admin/users"
            element={<AdminUsersPage />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;