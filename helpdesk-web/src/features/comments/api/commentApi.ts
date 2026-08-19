import { apiFetch } from "../../../lib/api";

import type {
  CommentResponse,
  CreateCommentRequest,
  UpdateCommentRequest,
} from "../types";

export function getComments(ticketId: number) {
  return apiFetch<CommentResponse[]>(
    `/tickets/${ticketId}/comments`,
  );
}

export function createComment(
  ticketId: number,
  request: CreateCommentRequest,
) {
  return apiFetch<CommentResponse>(
    `/tickets/${ticketId}/comments`,
    {
      method: "POST",
      body: JSON.stringify(request),
    },
  );
}

export function updateComment(
  id: number,
  request: UpdateCommentRequest,
) {
  return apiFetch<CommentResponse>(
    `/comments/${id}`,
    {
      method: "PUT",
      body: JSON.stringify(request),
    },
  );
}

export function deleteComment(id: number) {
  return apiFetch<void>(
    `/comments/${id}`,
    {
      method: "DELETE",
    },
  );
}