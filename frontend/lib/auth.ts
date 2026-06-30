import api from "./api";
import type { LoginRequest, CurrentUserResponse } from "@/types";

export async function login(data: LoginRequest): Promise<string> {
  const response = await api.post<string>("/auth/login", data);
  return response.data;
}

export async function getMe(): Promise<CurrentUserResponse> {
  const response = await api.get<CurrentUserResponse>("/auth/me");
  return response.data;
}
