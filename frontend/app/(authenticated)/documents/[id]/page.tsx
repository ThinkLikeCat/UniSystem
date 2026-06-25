"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/components/AuthProvider";
import { getDocumentById, sendToReview } from "@/lib/documents";
import { uploadAttachment, deleteAttachment } from "@/lib/attachments";
import { Button } from "@/components/ui/Button";
import type { DocumentDetailDto } from "@/types";
import {
  ArrowLeft,
  FileText,
  Calendar,
  User,
  Download,
  Trash2,
  Upload,
  Send,
  CheckCircle,
  XCircle,
  Clock,
  RefreshCw,
  MessageSquare,
} from "lucide-react";
import Link from "next/link";

export default function DocumentDetailPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { user } = useAuth();
  const [doc, setDoc] = useState<DocumentDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [sending, setSending] = useState(false);
  const [uploading, setUploading] = useState(false);

  const fetchDoc = async () => {
    try {
      const d = await getDocumentById(id);
      setDoc(d);
    } catch {
      setError("Ошибка загрузки документа");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDoc();
  }, [id]);

  if (!user) return null;

  const isStudent = user.role === "StudentProfile";
  const isSecretary = user.role === "Secretary";
  const isDean = user.role === "Dean";

  const canEdit =
    isStudent &&
    doc &&
    (doc.currentStatusName === "Черновик" ||
      doc.currentStatusName === "На доработку");

  const canSendToReview =
    isStudent && doc && doc.currentStatusName === "Черновик";

  const canReview = () => {
    if (!doc) return false;
    if (isSecretary && doc.currentStatusName === "На проверке секретаря")
      return true;
    if (isDean && doc.currentStatusName === "На проверке декана") return true;
    return false;
  };

  const handleSendToReview = async () => {
    if (!confirm("Отправить документ на проверку?")) return;
    setSending(true);
    try {
      await sendToReview(id);
      await fetchDoc();
    } catch {
      setError("Ошибка при отправке");
    } finally {
      setSending(false);
    }
  };

  const handleUploadAttachment = async (
    e: React.ChangeEvent<HTMLInputElement>
  ) => {
    const file = e.target.files?.[0];
    if (!file) return;
    if (file.size > 10 * 1024 * 1024) {
      setError("Файл не должен превышать 10 МБ");
      return;
    }
    setUploading(true);
    try {
      await uploadAttachment(id, file);
      await fetchDoc();
    } catch {
      setError("Ошибка при загрузке файла");
    } finally {
      setUploading(false);
    }
  };

  const handleDeleteAttachment = async (attachmentId: string) => {
    if (!confirm("Удалить вложение?")) return;
    try {
      await deleteAttachment(id, attachmentId);
      await fetchDoc();
    } catch {
      setError("Ошибка при удалении");
    }
  };

  if (loading) {
    return (
      <div className="flex-1 flex items-center justify-center">
        <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  if (error || !doc) {
    return (
      <div className="p-6 max-w-3xl mx-auto">
        <div className="bg-error-bg border border-error-border rounded-[16px] px-6 py-4 text-sm text-error-text">
          {error || "Документ не найден"}
        </div>
      </div>
    );
  }

  const statusHistory = [
    { label: "Создан", date: doc.createdAt, done: true },
    {
      label: "На проверке секретаря",
      date: doc.sendToReviewAt,
      done: !!doc.sendToReviewAt,
      current: doc.currentStatusName === "На проверке секретаря",
    },
    {
      label: "На проверке декана",
      date: doc.secretaryCheckAt,
      done: !!doc.secretaryCheckAt,
      current: doc.currentStatusName === "На проверке декана",
    },
    {
      label: "Утверждён / Отклонён",
      date: doc.deanCheckAt,
      done: !!doc.deanCheckAt,
      current:
        doc.currentStatusName === "Утверждён" ||
        doc.currentStatusName === "Отклонён",
    },
  ];

  let dynamicValuesObj: Record<string, string> = {};
  if (doc.dynamicValues) {
    try {
      dynamicValuesObj = JSON.parse(doc.dynamicValues);
    } catch {
      dynamicValuesObj = {};
    }
  }

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push("/documents")}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <div className="flex-1">
          <h1 className="text-2xl font-bold text-text-primary">
            {doc.documentTypeName}
          </h1>
          <p className="text-sm text-text-secondary">
            от {new Date(doc.createdAt).toLocaleDateString("ru-RU")}
          </p>
        </div>
        <span
          className="text-sm font-medium px-4 py-2"
          style={{
            borderRadius: "var(--radius-pill)",
            backgroundColor: `${getStatusColor(doc.currentStatusName)}15`,
            color: getStatusColor(doc.currentStatusName),
          }}
        >
          {doc.currentStatusName}
        </span>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="md:col-span-2 space-y-6">
          {/* Main info */}
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Информация о документе
            </h2>
            <div className="grid grid-cols-2 gap-4">
              <Field
                icon={FileText}
                label="Тип"
                value={doc.documentTypeName}
              />
              <Field icon={User} label="Автор" value={doc.authorName} />
              <Field
                icon={Calendar}
                label="Дата создания"
                value={new Date(doc.createdAt).toLocaleDateString("ru-RU")}
              />
              <Field
                icon={Calendar}
                label="Отправлен на проверку"
                value={
                  doc.sendToReviewAt
                    ? new Date(doc.sendToReviewAt).toLocaleDateString(
                        "ru-RU"
                      )
                    : "—"
                }
              />
            </div>

            {Object.keys(dynamicValuesObj).length > 0 && (
              <div className="mt-6 pt-4 border-t border-card-border">
                <h3 className="font-medium text-text-primary mb-3">
                  Содержание документа
                </h3>
                <div className="grid grid-cols-1 gap-3">
                  {Object.entries(dynamicValuesObj).map(([key, value]) => (
                    <div key={key}>
                      <p className="text-xs text-text-muted">
                        {getFieldLabel(key)}
                      </p>
                      <p className="text-sm text-text-primary whitespace-pre-wrap">
                        {value}
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            )}

            {doc.resolutionComment && (
              <div className="mt-6 pt-4 border-t border-card-border">
                <div className="flex items-start gap-3">
                  <MessageSquare className="w-5 h-5 text-text-muted mt-0.5 shrink-0" />
                  <div>
                    <p className="text-sm font-medium text-text-primary">
                      Резолюция
                    </p>
                    <p className="text-sm text-text-secondary mt-1">
                      {doc.resolutionComment}
                    </p>
                    {doc.resolvedByUserName && (
                      <p className="text-xs text-text-muted mt-1">
                        {doc.resolvedByUserName}
                      </p>
                    )}
                  </div>
                </div>
              </div>
            )}
          </div>

          {/* Attachments */}
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Вложения
            </h2>

            {doc.attachments.length === 0 ? (
              <p className="text-sm text-text-muted">Нет вложений</p>
            ) : (
              <div className="space-y-2">
                {doc.attachments.map((att) => (
                  <div
                    key={att.id}
                    className="flex items-center justify-between px-4 py-3 bg-hero-bg rounded-[12px]"
                  >
                    <div className="flex items-center gap-3 min-w-0">
                      <FileText className="w-4 h-4 text-text-muted shrink-0" />
                      <div className="min-w-0">
                        <p className="text-sm text-text-primary truncate">
                          {att.fileName}
                        </p>
                        <p className="text-xs text-text-muted">
                          {formatFileSize(att.fileSize)} ·{" "}
                          {new Date(att.uploadedAt).toLocaleDateString(
                            "ru-RU"
                          )}
                        </p>
                      </div>
                    </div>
                    <div className="flex items-center gap-2">
                      <a
                        href={`http://localhost:5000/api/documents/${id}/attachments/${att.id}`}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="p-2 hover:bg-card-border rounded-[8px] transition-colors"
                      >
                        <Download className="w-4 h-4 text-text-secondary" />
                      </a>
                      {canEdit && (
                        <button
                          onClick={() =>
                            handleDeleteAttachment(att.id)
                          }
                          className="p-2 hover:bg-error-bg rounded-[8px] transition-colors"
                        >
                          <Trash2 className="w-4 h-4 text-error-text" />
                        </button>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}

            {canEdit && (
              <label className="inline-flex items-center gap-2 mt-4 px-4 py-2 border border-card-border rounded-[12px] text-sm text-text-secondary hover:bg-table-hover cursor-pointer transition-colors">
                <Upload className="w-4 h-4" />
                {uploading ? "Загрузка..." : "Загрузить файл"}
                <input
                  type="file"
                  className="hidden"
                  onChange={handleUploadAttachment}
                  disabled={uploading}
                />
              </label>
            )}
          </div>

          {/* Actions */}
          <div className="flex gap-3">
            {canEdit && (
              <Link
                href={`/documents/${id}/edit`}
                className="inline-flex items-center gap-2 h-12 px-6 bg-card-bg border border-card-border text-text-primary rounded-[16px] font-semibold text-sm hover:bg-table-hover transition-colors"
              >
                <FileText className="w-4 h-4" />
                Редактировать
              </Link>
            )}

            {canSendToReview && (
              <Button
                onClick={handleSendToReview}
                loading={sending}
                className="inline-flex items-center gap-2"
              >
                <Send className="w-4 h-4" />
                Отправить на проверку
              </Button>
            )}

            {canReview() && (
              <Link
                href={`/documents/${id}/review`}
                className="inline-flex items-center gap-2 h-12 px-6 bg-primary text-white rounded-[16px] font-semibold text-sm shadow-[0px_8px_20px_rgba(37,99,235,0.35)] hover:bg-primary-hover transition-colors"
              >
                <CheckCircle className="w-4 h-4" />
                Проверить документ
              </Link>
            )}
          </div>
        </div>

        {/* Status Timeline */}
        <div
          className="bg-card-bg border border-card-border p-6 h-fit"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            Статус документа
          </h2>
          <div className="space-y-0">
            {statusHistory.map((step, idx) => {
              const Icon = step.current ? Clock : step.done ? CheckCircle : XCircle;
              return (
                <div key={step.label} className="flex gap-3 relative pb-6 last:pb-0">
                  {idx < statusHistory.length - 1 && (
                    <div className="absolute left-[11px] top-6 bottom-0 w-0.5 bg-divider" />
                  )}
                  <div
                    className="w-6 h-6 rounded-full flex items-center justify-center shrink-0 mt-0.5"
                    style={{
                      backgroundColor: step.done
                        ? "var(--color-status-approved)"
                        : step.current
                        ? "var(--color-status-secretary)"
                        : "var(--color-card-border)",
                    }}
                  >
                    <Icon className="w-3 h-3 text-white" />
                  </div>
                  <div className="min-w-0">
                    <p
                      className={`text-sm font-medium ${
                        step.done || step.current
                          ? "text-text-primary"
                          : "text-text-muted"
                      }`}
                    >
                      {step.label}
                    </p>
                    {step.date && (
                      <p className="text-xs text-text-muted">
                        {new Date(step.date).toLocaleDateString("ru-RU")}
                      </p>
                    )}
                  </div>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </div>
  );
}

function Field({
  icon: Icon,
  label,
  value,
}: {
  icon: React.ComponentType<{ className?: string }>;
  label: string;
  value: string;
}) {
  return (
    <div className="flex items-start gap-3">
      <Icon className="w-4 h-4 text-text-muted mt-0.5 shrink-0" />
      <div className="min-w-0">
        <p className="text-xs text-text-muted">{label}</p>
        <p className="text-sm font-medium text-text-primary truncate">
          {value}
        </p>
      </div>
    </div>
  );
}

function getStatusColor(statusName: string): string {
  const map: Record<string, string> = {
    Черновик: "var(--color-status-draft)",
    "На проверке секретаря": "var(--color-status-secretary)",
    "На доработку": "var(--color-status-rework)",
    "На проверке декана": "var(--color-status-dean)",
    Утверждён: "var(--color-status-approved)",
    Отклонён: "var(--color-status-rejected)",
  };
  return map[statusName] || "var(--color-text-muted)";
}

function formatFileSize(bytes: number): string {
  if (bytes < 1024) return `${bytes} Б`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} КБ`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} МБ`;
}

function getFieldLabel(key: string): string {
  const labels: Record<string, string> = {
    student_name: "Студент",
    group_name: "Группа",
    course: "Курс",
    specialty: "Специальность",
    reason: "Причина",
    subject: "Предмет",
    faculty: "Факультет",
    single_date: "Дата",
    start_date: "Дата начала",
    end_date: "Дата окончания",
  };
  return labels[key] || key;
}
