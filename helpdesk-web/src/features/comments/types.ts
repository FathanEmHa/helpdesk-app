export interface CreateCommentRequest {
  content: string;
}

export interface UpdateCommentRequest {
  content: string;
  version: number;
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