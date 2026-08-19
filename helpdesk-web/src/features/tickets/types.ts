export type TicketStatus =
  | "Open"
  | "InProgress"
  | "Resolved"
  | "Closed";

export type TicketPriority =
  | "Low"
  | "Medium"
  | "High"
  | "Critical";

export type TicketSortBy =
  | "CreatedAt"
  | "Title"
  | "Priority"
  | "Status";

export interface TicketQueryRequest {
  page?: number;
  pageSize?: number;
  search?: string;
  status?: TicketStatus;
  priority?: TicketPriority;
  sortBy?: TicketSortBy;
  descending?: boolean;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
}

export interface UpdateMyTicketRequest {
  title: string;
  description: string;
  version: number;
}

export interface UpdateAdminTicketRequest {
  priority: TicketPriority;
  status: TicketStatus;
  version: number;
}

export interface TicketListResponse {
  id: number;
  ticketNumber: string;
  title: string;
  status: TicketStatus;
  priority: TicketPriority;
  userId: number;
  userName: string;
  commentCount: number;
  createdAt: string;
}

export interface CommentResponse {
  id: number;
  content: string;
  ticketId: number;
  userId: number;
  userName: string;
  createdAt: string;
  updatedAt: string | null;
  version: number;
}

export interface TicketDetailResponse {
  id: number;
  ticketNumber: string;
  title: string;
  description: string;
  status: TicketStatus;
  priority: TicketPriority;
  userId: number;
  userName: string;
  createdAt: string;
  updatedAt: string | null;
  version: number;
  comments: CommentResponse[];
}

export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}