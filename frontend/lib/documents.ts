import api from "./api";
import type {
  DocumentListItemDto,
  DocumentDetailDto,
  CreateDocumentCommand,
  UpdateDocumentDynamicValuesCommand,
  SecretaryReviewCommand,
  DeanReviewCommand,
  TemplatePreviewDto,
} from "@/types";

export async function getDocuments(params?: {
  documentTypeId?: number;
  statusId?: number;
  authorId?: string;
}): Promise<DocumentListItemDto[]> {
  const response = await api.get<DocumentListItemDto[]>("/documents", {
    params,
  });
  return response.data;
}

export async function getDocumentById(
  id: string
): Promise<DocumentDetailDto> {
  const response = await api.get<DocumentDetailDto>(`/documents/${id}`);
  return response.data;
}

export async function createDocument(
  data: CreateDocumentCommand
): Promise<string> {
  const response = await api.post<string>("/documents", data);
  return response.data;
}

export async function updateDocumentDynamicValues(
  id: string,
  data: UpdateDocumentDynamicValuesCommand
): Promise<void> {
  await api.put(`/documents/${id}/dynamic-values`, data);
}

export async function sendToReview(id: string): Promise<void> {
  await api.post(`/documents/${id}/send-to-review`);
}

export async function secretaryReview(
  id: string,
  data: SecretaryReviewCommand
): Promise<void> {
  await api.post(`/documents/${id}/secretary-review`, data);
}

export async function deanReview(
  id: string,
  data: DeanReviewCommand
): Promise<void> {
  await api.post(`/documents/${id}/dean-review`, data);
}

export async function deleteDraftDocument(id: string): Promise<void> {
  await api.delete(`/documents/${id}`);
}

export async function getDocumentTypeTemplate(
  id: number
): Promise<TemplatePreviewDto> {
  const response = await api.get<TemplatePreviewDto>(
    `/document-types/${id}/template`
  );
  return response.data;
}
