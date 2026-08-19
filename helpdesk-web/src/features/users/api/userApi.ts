import { apiFetch } from "../../../lib/api";

import type {
  CreateUserRequest,
  PagedResponse,
  UpdateProfileRequest,
  UpdateUserRequest,
  UserQueryRequest,
  UserResponse,
} from "../types";

function buildQueryParams(
  query: UserQueryRequest,
): string {
  const params = new URLSearchParams();

  if (query.page !== undefined) {
    params.set("page", String(query.page));
  }

  if (query.pageSize !== undefined) {
    params.set("pageSize", String(query.pageSize));
  }

  if (query.search) {
    params.set("search", query.search);
  }

  if (query.role) {
    params.set("role", query.role);
  }

  if (query.status) {
    params.set("status", query.status);
  }

  if (query.sortBy) {
    params.set("sortBy", query.sortBy);
  }

  if (query.descending !== undefined) {
    params.set(
      "descending",
      String(query.descending),
    );
  }

  const queryString = params.toString();

  return queryString
    ? `?${queryString}`
    : "";
}

// =========================
// Admin
// =========================

export function getUsers(
  query: UserQueryRequest = {},
) {
  const queryString =
    buildQueryParams(query);

  return apiFetch<
    PagedResponse<UserResponse>
  >(`/users${queryString}`);
}

export function getUserById(id: number) {
  return apiFetch<UserResponse>(
    `/users/${id}`,
  );
}

export function createUser(
  request: CreateUserRequest,
) {
  return apiFetch<UserResponse>(
    "/users",
    {
      method: "POST",
      body: JSON.stringify(request),
    },
  );
}

export function updateUser(
  id: number,
  request: UpdateUserRequest,
) {
  return apiFetch<UserResponse>(
    `/users/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(request),
    },
  );
}

export function deleteUser(id: number) {
  return apiFetch<void>(
    `/users/${id}`,
    {
      method: "DELETE",
    },
  );
}

// =========================
// Current User
// =========================

export function getCurrentProfile() {
  return apiFetch<UserResponse>(
    "/users/me",
  );
}

export function updateProfile(
  request: UpdateProfileRequest,
) {
  return apiFetch<UserResponse>(
    "/users/me",
    {
      method: "PUT",
      body: JSON.stringify(request),
    },
  );
}