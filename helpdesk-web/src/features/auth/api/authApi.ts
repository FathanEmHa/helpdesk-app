import { apiFetch } from "../../../lib/api";
import type {
  LoginRequest,
  AuthResponse,
} from "../types";

export function login(request: LoginRequest) {
  return apiFetch<AuthResponse>("/auth/login", {
    method: "POST",
    body: JSON.stringify(request),
  });
}