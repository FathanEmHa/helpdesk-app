import { Outlet } from "react-router";

export default function MainLayout() {
  return (
    <div className="min-h-screen bg-background text-foreground">
      <header className="h-14 border-b">
        Navbar
      </header>

      <div className="flex min-h-[calc(100vh-3.5rem)]">
        <aside className="w-64 shrink-0 border-r p-4">
          Sidebar
        </aside>

        <main className="flex-1 p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}