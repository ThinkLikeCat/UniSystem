import api from "./api";
import type {
  CurrentUserResponse,
  UpdateStudentProfileCommand,
  UpdateStaffProfileCommand,
  ChangePasswordCommand,
} from "@/types";

export async function getProfile(): Promise<CurrentUserResponse> {
  const response = await api.get<CurrentUserResponse>("/profile");
  return response.data;
}

export async function updateStudentProfile(
  data: UpdateStudentProfileCommand
): Promise<void> {
  await api.put("/profile/student", data);
}

export async function updateStaffProfile(
  data: UpdateStaffProfileCommand
): Promise<void> {
  await api.put("/profile/staff", data);
}

export async function changePassword(
  data: ChangePasswordCommand
): Promise<void> {
  await api.post("/profile/change-password", data);
}
