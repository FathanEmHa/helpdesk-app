import { apiFetch } from "../../../lib/api";
import type {
  AuthResponse,
  LoginRequest,
} from "../types";

export async function login(
  request: LoginRequest,
): Promise<AuthResponse> {
  return apiFetch<AuthResponse>("/auth/login", {
    method: "POST",
    body: JSON.stringify(request),
  });
}