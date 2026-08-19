import { getStoredAuth } from "../features/auth/authStorage";
import { ApiError } from "./apiError";

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

  let response: Response;

  try {
    response = await fetch(
      `${API_URL}${endpoint}`,
      {
        ...options,
        headers: {
          "Content-Type": "application/json",

          ...(auth?.token && {
            Authorization: `Bearer ${auth.token}`,
          }),

          ...options?.headers,
        },
      },
    );
  } catch {
    throw new ApiError(
      "Unable to connect to the server.",
      0,
    );
  }

  if (!response.ok) {
    let errorData: {
      message?: string;
      errors?: string[];
    } = {};

    try {
      errorData = await response.json();
    } catch {
      // Response tidak memiliki JSON body.
    }

    if (response.status === 401) {
      unauthorizedHandler?.();
    }

    throw new ApiError(
      errorData.message ??
        "Something went wrong.",
      response.status,
      errorData.errors ?? [],
    );
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return response.json();
}