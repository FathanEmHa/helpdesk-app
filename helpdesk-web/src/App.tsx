import { BrowserRouter, Navigate, Route, Routes } from "react-router";
import MainLayout from "./components/layout/MainLayout";
import LoginPage from "./features/auth/pages/LoginPage";
import DashboardPage from "./features/dashboard/pages/DashboardPage";
import TicketsPage from "./features/tickets/pages/TicketsPage";
import TicketDetailPage from "./features/tickets/pages/TicketDetailPage";

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
            path="/tickets/:id"
            element={<TicketDetailPage />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;