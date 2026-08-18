import { getStoredAuth } from "../features/auth/authStorage";

const API_URL = import.meta.env.VITE_API_URL;

let unauthorizedHandler: (() => void) | null = null;

export function setUnauthorizedHandler(
  handler: () => void,
) {
  unauthorizedHandler = handler;
}

export async function apiFetch<T>(
  endpoint: string,
  options?: RequestInit,
): Promise<T> {
  const auth = getStoredAuth();

  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",

      ...(auth?.token && {
        Authorization: `Bearer ${auth.token}`,
      }),

      ...options?.headers,
    },
  });

  if (!response.ok) {
    const errorData = await response.json();

    if (response.status === 401) {
      unauthorizedHandler?.();

      throw new Error(
        errorData.message ?? "Unauthorized.",
      );
    }

    throw new Error(
      errorData.message ?? "Request failed.",
    );
  }

  return response.json();
}