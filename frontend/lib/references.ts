import api from "./api";
import type {
  DepartmentDto,
  SpecialtyDto,
  AcademicGroupDto,
  SubjectDto,
  DocumentTypeDto,
  DocumentStatusDto,
  StudentStatusDto,
} from "@/types";

// ─── Departments ─────────────────────────────────────
export async function getDepartments(): Promise<DepartmentDto[]> {
  const response = await api.get<DepartmentDto[]>("/departments");
  return response.data;
}

export async function getDepartmentById(
  id: number
): Promise<DepartmentDto> {
  const response = await api.get<DepartmentDto>(`/departments/${id}`);
  return response.data;
}

export async function createDepartment(
  name: string
): Promise<number> {
  const response = await api.post<number>("/departments", { name });
  return response.data;
}

export async function updateDepartment(
  id: number,
  name: string
): Promise<void> {
  await api.put(`/departments/${id}`, { name });
}

export async function deleteDepartment(id: number): Promise<void> {
  await api.delete(`/departments/${id}`);
}

// ─── Specialties ─────────────────────────────────────
export async function getSpecialties(): Promise<SpecialtyDto[]> {
  const response = await api.get<SpecialtyDto[]>("/specialties");
  return response.data;
}

export async function getSpecialtyById(
  id: number
): Promise<SpecialtyDto> {
  const response = await api.get<SpecialtyDto>(`/specialties/${id}`);
  return response.data;
}

export async function createSpecialty(data: {
  name: string;
  code: string;
  maxDurationInYears: number;
}): Promise<number> {
  const response = await api.post<number>("/specialties", data);
  return response.data;
}

export async function updateSpecialty(
  id: number,
  data: { name: string; code: string; maxDurationInYears: number }
): Promise<void> {
  await api.put(`/specialties/${id}`, data);
}

export async function deleteSpecialty(id: number): Promise<void> {
  await api.delete(`/specialties/${id}`);
}

// ─── Academic Groups ─────────────────────────────────
export async function getAcademicGroups(): Promise<AcademicGroupDto[]> {
  const response = await api.get<AcademicGroupDto[]>("/academic-groups");
  return response.data;
}

export async function getAcademicGroupById(
  id: number
): Promise<AcademicGroupDto> {
  const response = await api.get<AcademicGroupDto>(
    `/academic-groups/${id}`
  );
  return response.data;
}

export async function createAcademicGroup(data: {
  name: string;
  maxCount: number;
  course: number;
  specialtyId: number;
}): Promise<number> {
  const response = await api.post<number>("/academic-groups", data);
  return response.data;
}

export async function updateAcademicGroup(
  id: number,
  data: { name: string; maxCount: number; course: number; specialtyId: number }
): Promise<void> {
  await api.put(`/academic-groups/${id}`, data);
}

export async function deleteAcademicGroup(id: number): Promise<void> {
  await api.delete(`/academic-groups/${id}`);
}

// ─── Subjects ────────────────────────────────────────
export async function getSubjects(): Promise<SubjectDto[]> {
  const response = await api.get<SubjectDto[]>("/subjects");
  return response.data;
}

export async function getSubjectById(id: number): Promise<SubjectDto> {
  const response = await api.get<SubjectDto>(`/subjects/${id}`);
  return response.data;
}

export async function createSubject(name: string): Promise<number> {
  const response = await api.post<number>("/subjects", { name });
  return response.data;
}

export async function updateSubject(
  id: number,
  name: string
): Promise<void> {
  await api.put(`/subjects/${id}`, { name });
}

export async function deleteSubject(id: number): Promise<void> {
  await api.delete(`/subjects/${id}`);
}

// ─── Document Types ──────────────────────────────────
export async function getDocumentTypes(): Promise<DocumentTypeDto[]> {
  const response = await api.get<DocumentTypeDto[]>("/document-types");
  return response.data;
}

export async function getDocumentTypeById(
  id: number
): Promise<DocumentTypeDto> {
  const response = await api.get<DocumentTypeDto>(
    `/document-types/${id}`
  );
  return response.data;
}

export async function createDocumentType(data: {
  name: string;
  requiresAttachments: boolean;
  templateText: string;
}): Promise<number> {
  const response = await api.post<number>("/document-types", data);
  return response.data;
}

export async function updateDocumentType(
  id: number,
  data: { name: string; requiresAttachments: boolean; templateText: string }
): Promise<void> {
  await api.put(`/document-types/${id}`, data);
}

export async function deleteDocumentType(id: number): Promise<void> {
  await api.delete(`/document-types/${id}`);
}

// ─── Document Statuses ───────────────────────────────
export async function getDocumentStatuses(): Promise<
  DocumentStatusDto[]
> {
  const response = await api.get<DocumentStatusDto[]>(
    "/document-statuses"
  );
  return response.data;
}

export async function getDocumentStatusById(
  id: number
): Promise<DocumentStatusDto> {
  const response = await api.get<DocumentStatusDto>(
    `/document-statuses/${id}`
  );
  return response.data;
}

export async function createDocumentStatus(
  name: string
): Promise<number> {
  const response = await api.post<number>("/document-statuses", {
    name,
  });
  return response.data;
}

export async function updateDocumentStatus(
  id: number,
  name: string
): Promise<void> {
  await api.put(`/document-statuses/${id}`, { name });
}

export async function deleteDocumentStatus(id: number): Promise<void> {
  await api.delete(`/document-statuses/${id}`);
}

// ─── Student Statuses ────────────────────────────────
export async function getStudentStatuses(): Promise<
  StudentStatusDto[]
> {
  const response = await api.get<StudentStatusDto[]>("/student-statuses");
  return response.data;
}

export async function getStudentStatusById(
  id: number
): Promise<StudentStatusDto> {
  const response = await api.get<StudentStatusDto>(
    `/student-statuses/${id}`
  );
  return response.data;
}

export async function createStudentStatus(
  name: string
): Promise<number> {
  const response = await api.post<number>("/student-statuses", {
    name,
  });
  return response.data;
}

export async function updateStudentStatus(
  id: number,
  name: string
): Promise<void> {
  await api.put(`/student-statuses/${id}`, { name });
}

export async function deleteStudentStatus(id: number): Promise<void> {
  await api.delete(`/student-statuses/${id}`);
}
