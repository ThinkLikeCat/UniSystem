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

export async function downloadAttachment(
  documentId: string,
  attachmentId: string,
  fileName: string
): Promise<void> {
  const response = await api.get(
    `/documents/${documentId}/attachments/${attachmentId}`,
    { responseType: "blob" }
  );
  const url = URL.createObjectURL(response.data);
  const a = document.createElement("a");
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}

export async function deleteAttachment(
  documentId: string,
  attachmentId: string
): Promise<void> {
  await api.delete(`/documents/${documentId}/attachments/${attachmentId}`);
}
