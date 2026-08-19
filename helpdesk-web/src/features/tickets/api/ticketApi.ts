import { apiFetch } from "../../../lib/api";

import type {
  CreateTicketRequest,
  PagedResponse,
  TicketDetailResponse,
  TicketListResponse,
  TicketQueryRequest,
  UpdateAdminTicketRequest,
  UpdateMyTicketRequest,
} from "../types";

function buildQueryParams(
  query: TicketQueryRequest,
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

  if (query.status) {
    params.set("status", query.status);
  }

  if (query.priority) {
    params.set("priority", query.priority);
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

export function getTickets(
  query: TicketQueryRequest = {},
) {
  const queryString =
    buildQueryParams(query);

  return apiFetch<
    PagedResponse<TicketListResponse>
  >(`/tickets${queryString}`);
}

export function getTicketById(id: number) {
  return apiFetch<TicketDetailResponse>(
    `/tickets/${id}`,
  );
}

export function createTicket(
  request: CreateTicketRequest,
) {
  return apiFetch<TicketDetailResponse>(
    "/tickets",
    {
      method: "POST",
      body: JSON.stringify(request),
    },
  );
}

export function updateMyTicket(
  id: number,
  request: UpdateMyTicketRequest,
) {
  return apiFetch<TicketDetailResponse>(
    `/tickets/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(request),
    },
  );
}

export function deleteTicket(id: number) {
  return apiFetch<void>(
    `/tickets/${id}`,
    {
      method: "DELETE",
    },
  );
}

export function updateAdminTicket(
  id: number,
  request: UpdateAdminTicketRequest,
) {
  return apiFetch<TicketDetailResponse>(
    `/admin/tickets/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(request),
    },
  );
}