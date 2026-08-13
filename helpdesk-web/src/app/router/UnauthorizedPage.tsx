import { Link } from "react-router";

function UnauthorizedPage() {
  return (
    <main className="flex min-h-screen items-center justify-center px-4">
      <div className="text-center">
        <h1 className="text-3xl font-semibold">
          Access Denied
        </h1>

        <p className="mt-2 text-sm text-muted-foreground">
          You don't have permission to access this page.
        </p>

        <Link
          to="/dashboard"
          className="mt-6 inline-flex rounded-md bg-primary px-4 py-2 text-sm font-medium text-primary-foreground"
        >
          Back to Dashboard
        </Link>
      </div>
    </main>
  );
}

export default UnauthorizedPage;