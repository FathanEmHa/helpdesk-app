export type Role =
  | "User"
  | "Admin";

export type UserStatus =
  | "Active"
  | "Inactive";

export type UserSortBy =
  | "Name"
  | "Email"
  | "Role"
  | "Status"
  | "CreatedAt";

export interface UserQueryRequest {
  page?: number;
  pageSize?: number;
  search?: string;
  role?: Role;
  status?: UserStatus;
  sortBy?: UserSortBy;
  descending?: boolean;
}

export interface CreateUserRequest {
  name: string;
  email: string;
  password: string;
}

export interface UpdateUserRequest {
  status: UserStatus;
  role: Role;
  version: number;
}

export interface UpdateProfileRequest {
  name: string;
  email: string;
  password?: string;
  version: number;
}

export interface UserResponse {
  id: number;
  name: string;
  email: string;
  role: Role;
  status: UserStatus;
  createdAt: string;
  updatedAt: string | null;
  version: number;
}