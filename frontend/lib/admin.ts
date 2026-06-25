import api from "./api";
import type {
  UserListItemDto,
  UserDetailDto,
  RegisterUserCommand,
  StatisticsSummaryDto,
  MonthlyStatDto,
  TypeStatDto,
  ResolutionStatDto,
  StatusStatDto,
} from "@/types";

// ─── Users ───────────────────────────────────────────
export async function getUsers(
  roleFilter?: string
): Promise<UserListItemDto[]> {
  const response = await api.get<UserListItemDto[]>("/admin/users", {
    params: roleFilter ? { roleFilter } : undefined,
  });
  return response.data;
}

export async function getUserById(id: string): Promise<UserDetailDto> {
  const response = await api.get<UserDetailDto>(`/admin/users/${id}`);
  return response.data;
}

export async function createUser(
  data: RegisterUserCommand
): Promise<string> {
  const response = await api.post<string>("/admin/users", data);
  return response.data;
}

export async function updateUser(
  id: string,
  data: {
    firstName?: string;
    lastName?: string;
    patronymic?: string | null;
    sex?: string;
    role?: string;
  }
): Promise<void> {
  await api.put(`/admin/users/${id}`, data);
}

export async function deleteUser(id: string): Promise<void> {
  await api.delete(`/admin/users/${id}`);
}

export async function resetPassword(
  id: string,
  newPassword: string
): Promise<void> {
  await api.post(`/admin/users/${id}/reset-password`, { newPassword });
}

// ─── Statistics ──────────────────────────────────────
export async function getStatisticsSummary(): Promise<StatisticsSummaryDto> {
  const response = await api.get<StatisticsSummaryDto>(
    "/admin/statistics/summary"
  );
  return response.data;
}

export async function getStatisticsByMonth(): Promise<MonthlyStatDto[]> {
  const response = await api.get<MonthlyStatDto[]>(
    "/admin/statistics/by-month"
  );
  return response.data;
}

export async function getStatisticsByType(): Promise<TypeStatDto[]> {
  const response = await api.get<TypeStatDto[]>(
    "/admin/statistics/by-type"
  );
  return response.data;
}

export async function getStatisticsResolution(): Promise<ResolutionStatDto> {
  const response = await api.get<ResolutionStatDto>(
    "/admin/statistics/resolution"
  );
  return response.data;
}

export async function getStatisticsByStatus(): Promise<StatusStatDto[]> {
  const response = await api.get<StatusStatDto[]>(
    "/admin/statistics/by-status"
  );
  return response.data;
}
