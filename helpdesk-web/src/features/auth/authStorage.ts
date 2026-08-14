import type { AuthResponse } from "./types";

const AUTH_STORAGE_KEY = "helpdesk_auth";

export function saveAuth(auth: AuthResponse) {
  localStorage.setItem(
    AUTH_STORAGE_KEY,
    JSON.stringify(auth),
  );
}

export function getStoredAuth(): AuthResponse | null {
  const storedAuth =
    localStorage.getItem(AUTH_STORAGE_KEY);

  if (!storedAuth) {
    return null;
  }

  try {
    return JSON.parse(storedAuth) as AuthResponse;
  } catch {
    localStorage.removeItem(AUTH_STORAGE_KEY);

    return null;
  }
}

export function clearStoredAuth() {
  localStorage.removeItem(AUTH_STORAGE_KEY);
}