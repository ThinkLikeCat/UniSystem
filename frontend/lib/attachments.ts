import api from "./api";

export async function uploadAttachment(
  documentId: string,
  file: File
): Promise<string> {
  const formData = new FormData();
  formData.append("file", file);
  const response = await api.post<string>(
    `/documents/${documentId}/attachments`,
    formData,
    { headers: { "Content-Type": "multipart/form-data" } }
  );
  return response.data;
}

export async function getAttachmentDownloadUrl(
  documentId: string,
  attachmentId: string
): Promise<string> {
  return `${api.defaults.baseURL}/documents/${documentId}/attachments/${attachmentId}`;
}

export async function deleteAttachment(
  documentId: string,
  attachmentId: string
): Promise<void> {
  await api.delete(`/documents/${documentId}/attachments/${attachmentId}`);
}
