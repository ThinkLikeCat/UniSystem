import api from "./api";
import type { StaffSubjectDto } from "@/types";

export async function getStaffSubjects(
  staffId: string
): Promise<StaffSubjectDto[]> {
  const response = await api.get<StaffSubjectDto[]>(
    `/staff-subjects/${staffId}`
  );
  return response.data;
}

export async function createStaffSubject(
  staffId: string,
  subjectId: number
): Promise<void> {
  await api.post("/staff-subjects", { staffId, subjectId });
}

export async function deleteStaffSubject(
  staffId: string,
  subjectId: number
): Promise<void> {
  await api.delete(`/staff-subjects/${staffId}/${subjectId}`);
}
