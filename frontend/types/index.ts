// ─── Auth ────────────────────────────────────────────
export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

// ─── Profile / CurrentUser ───────────────────────────
export interface CurrentUserResponse {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  patronymic?: string;
  fullName: string;
  sex: Sex;
  role: SystemRoleName;
  iconPath?: string | null;
  studentProfile?: StudentProfileResponse | null;
  staffProfile?: StaffProfileResponse | null;
}

export interface StudentProfileResponse {
  studentTicket: string;
  academicGroupId: number;
  academicGroupName?: string | null;
  studentStatusId: number;
  studentStatusName?: string | null;
}

export interface StaffProfileResponse {
  departmentId: number;
  departmentName?: string | null;
  academicGroupId?: number | null;
  academicGroupName?: string | null;
}

// ─── Documents ───────────────────────────────────────
export interface DocumentListItemDto {
  id: string;
  authorName: string;
  documentTypeName: string;
  currentStatusName: string;
  createdAt: string;
}

export interface DocumentDetailDto {
  id: string;
  documentTypeId: number;
  authorName: string;
  documentTypeName: string;
  currentStatusName: string;
  secretaryStatusName?: string | null;
  deanStatusName?: string | null;
  createdAt: string;
  sendToReviewAt?: string | null;
  secretaryCheckAt?: string | null;
  deanCheckAt?: string | null;
  dynamicValues?: string | null;
  resolutionComment?: string | null;
  resolvedByUserName?: string | null;
  attachments: AttachmentDto[];
}

export interface AttachmentDto {
  id: string;
  fileName: string;
  fileSize: number;
  uploadedAt: string;
}

export interface TemplatePreviewDto {
  documentTypeId: number;
  documentTypeName: string;
  systemFields: Record<string, string>;
  userFields: string[];
}

// ─── References (Справочники) ────────────────────────
export interface DepartmentDto {
  id: number;
  name: string;
}

export interface SpecialtyDto {
  id: number;
  name: string;
  code: string;
  maxDurationInYears: number;
}

export interface AcademicGroupDto {
  id: number;
  name: string;
  maxCount: number;
  course: number;
  specialtyId: number;
  specialtyName: string;
}

export interface SubjectDto {
  id: number;
  name: string;
}

export interface DocumentTypeDto {
  id: number;
  name: string;
  requiresAttachments: boolean;
  templateText: string;
}

export interface DocumentStatusDto {
  id: number;
  name: string;
}

export interface StudentStatusDto {
  id: number;
  name: string;
}

// ─── StaffSubjects ───────────────────────────────────
export interface StaffSubjectDto {
  staffId: string;
  subjectId: number;
  subjectName: string;
}

// ─── Admin ───────────────────────────────────────────
export interface UserListItemDto {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  patronymic?: string | null;
  fullName: string;
  role: string;
}

export interface UserDetailDto {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  patronymic?: string | null;
  fullName: string;
  sex: string;
  role: string;
  studentTicket?: string | null;
  academicGroupId?: number | null;
  academicGroupName?: string | null;
  studentStatusId?: number | null;
  studentStatusName?: string | null;
  departmentId?: number | null;
  departmentName?: string | null;
  curatorGroupId?: number | null;
  curatorGroupName?: string | null;
}

// ─── Statistics ──────────────────────────────────────
export interface StatisticsSummaryDto {
  totalDocuments: number;
  draft: number;
  underSecretaryReview: number;
  rework: number;
  underDeanReview: number;
  approved: number;
  rejected: number;
}

export interface MonthlyStatDto {
  year: number;
  month: number;
  count: number;
}

export interface TypeStatDto {
  documentTypeId: number;
  documentTypeName: string;
  count: number;
}

export interface ResolutionStatDto {
  withResolution: number;
  withoutResolution: number;
  total: number;
}

export interface StatusStatDto {
  statusId: number;
  statusName: string;
  count: number;
}

// ─── Enums ───────────────────────────────────────────
export type Sex = "Male" | "Female" | "Other";

export type SystemRoleName =
  | "StudentProfile"
  | "StaffProfile"
  | "Dean"
  | "Secretary"
  | "Curator"
  | "Admin";

export type SecretaryDecision = "ApproveToDean" | "ReturnToRework" | "Reject";
export type DeanDecision = "Approve" | "Reject";

// ─── Command types (request bodies) ──────────────────
export interface RegisterUserCommand {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  patronymic?: string | null;
  sex: Sex;
  role: SystemRoleName;
  studentTicket?: string | null;
  academicGroupId?: number | null;
  studentStatusId?: number | null;
  departmentId?: number | null;
}

export interface ChangePasswordCommand {
  currentPassword: string;
  newPassword: string;
}

export interface UpdateStudentProfileCommand {
  studentTicket?: string | null;
}

export interface UpdateStaffProfileCommand {
  departmentId?: number | null;
}

export interface CreateDocumentCommand {
  documentTypeId: number;
  dynamicValues?: string | null;
}

export interface UpdateDocumentDynamicValuesCommand {
  documentId: string;
  dynamicValues: string;
}

export interface SecretaryReviewCommand {
  documentId: string;
  decision: SecretaryDecision;
  resolutionComment?: string | null;
}

export interface DeanReviewCommand {
  documentId: string;
  decision: DeanDecision;
  resolutionComment?: string | null;
}

export interface CreateStaffSubjectCommand {
  staffId: string;
  subjectId: number;
}

// ─── Role display names ──────────────────────────────
export const ROLE_LABELS: Record<SystemRoleName, string> = {
  StudentProfile: "Студент",
  StaffProfile: "Сотрудник",
  Dean: "Декан",
  Secretary: "Секретарь",
  Curator: "Куратор",
  Admin: "Администратор",
};

// ─── Document status display names ───────────────────
export const DOCUMENT_STATUS_COLORS: Record<string, string> = {
  Черновик: "var(--color-status-draft)",
  "На проверке секретаря": "var(--color-status-secretary)",
  "На доработку": "var(--color-status-rework)",
  "На проверке декана": "var(--color-status-dean)",
  Утверждён: "var(--color-status-approved)",
  Отклонён: "var(--color-status-rejected)",
};
