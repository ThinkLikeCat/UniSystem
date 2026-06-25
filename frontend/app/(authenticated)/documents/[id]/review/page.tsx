"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/components/AuthProvider";
import {
  getDocumentById,
  secretaryReview,
  deanReview,
} from "@/lib/documents";
import { Button } from "@/components/ui/Button";
import type { DocumentDetailDto } from "@/types";
import { ArrowLeft, CheckCircle, XCircle, RefreshCw, MessageSquare } from "lucide-react";
import Link from "next/link";

export default function ReviewDocumentPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { user } = useAuth();
  const [doc, setDoc] = useState<DocumentDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [comment, setComment] = useState("");

  const isSecretary = user?.role === "Secretary";
  const isDean = user?.role === "Dean";

  useEffect(() => {
    const load = async () => {
      try {
        const d = await getDocumentById(id);
        setDoc(d);
      } catch {
        setError("Ошибка загрузки документа");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [id]);

  if (!user) return null;

  if (!isSecretary && !isDean) {
    return (
      <div className="p-6 max-w-3xl mx-auto">
        <div className="bg-error-bg border border-error-border rounded-[16px] px-6 py-4 text-sm text-error-text">
          У вас нет прав для проверки документов
        </div>
      </div>
    );
  }

  const handleReview = async (decision: string) => {
    setSaving(true);
    setError("");

    try {
      if (isSecretary) {
        await secretaryReview(id, {
          documentId: id,
          decision: decision as "ApproveToDean" | "ReturnToRework" | "Reject",
          resolutionComment: comment || null,
        });
      } else {
        await deanReview(id, {
          documentId: id,
          decision: decision as "Approve" | "Reject",
          resolutionComment: comment || null,
        });
      }
      router.push(`/documents/${id}`);
    } catch {
      setError("Ошибка при проверке документа");
    } finally {
      setSaving(false);
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

  // parse dynamic values for preview
  let dynamicValuesObj: Record<string, string> = {};
  if (doc.dynamicValues) {
    try {
      dynamicValuesObj = JSON.parse(doc.dynamicValues);
    } catch {
      // ignore
    }
  }

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push(`/documents/${id}`)}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <div className="flex-1">
          <h1 className="text-2xl font-bold text-text-primary">
            Проверка документа
          </h1>
          <p className="text-sm text-text-secondary">
            {doc.documentTypeName} · {doc.authorName}
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
          {/* Document Preview */}
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Содержание документа
            </h2>

            <div className="grid grid-cols-2 gap-4 mb-4">
              <InfoField label="Автор" value={doc.authorName} />
              <InfoField label="Тип" value={doc.documentTypeName} />
              <InfoField
                label="Создан"
                value={new Date(doc.createdAt).toLocaleDateString("ru-RU")}
              />
              <InfoField
                label="Отправлен"
                value={
                  doc.sendToReviewAt
                    ? new Date(doc.sendToReviewAt).toLocaleDateString("ru-RU")
                    : "—"
                }
              />
            </div>

            {Object.keys(dynamicValuesObj).length > 0 && (
              <div className="border-t border-card-border pt-4 mt-4">
                {Object.entries(dynamicValuesObj).map(([key, val]) => (
                  <div key={key} className="mb-3">
                    <p className="text-xs text-text-muted">
                      {getFieldLabel(key)}
                    </p>
                    <p className="text-sm text-text-primary whitespace-pre-wrap">
                      {val}
                    </p>
                  </div>
                ))}
              </div>
            )}

            {doc.attachments.length > 0 && (
              <div className="border-t border-card-border pt-4 mt-4">
                <h3 className="text-sm font-medium text-text-primary mb-2">
                  Вложения ({doc.attachments.length})
                </h3>
                <div className="space-y-1">
                  {doc.attachments.map((att) => (
                    <div
                      key={att.id}
                      className="flex items-center gap-2 text-sm"
                    >
                      <span className="text-text-secondary">
                        {att.fileName}
                      </span>
                      <a
                        href={`http://localhost:5000/api/documents/${id}/attachments/${att.id}`}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="text-primary hover:underline ml-auto"
                      >
                        Скачать
                      </a>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>

          {/* Review Form */}
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Решение
            </h2>

            <div className="space-y-4">
              <div className="flex flex-col gap-1.5">
                <label className="text-sm text-text-secondary">
                  Комментарий (необязательно)
                </label>
                <textarea
                  value={comment}
                  onChange={(e) => setComment(e.target.value)}
                  rows={3}
                  placeholder="Введите комментарий к решению..."
                  className="w-full px-4 py-3 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200 resize-none"
                />
              </div>

              {error && (
                <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                  {error}
                </div>
              )}

              {isSecretary && (
                <div className="flex flex-wrap gap-3">
                  <Button
                    onClick={() => handleReview("ApproveToDean")}
                    loading={saving}
                    className="inline-flex items-center gap-2"
                  >
                    <CheckCircle className="w-4 h-4" />
                    Одобрить (→ декану)
                  </Button>
                  <Button
                    onClick={() => handleReview("ReturnToRework")}
                    loading={saving}
                    variant="ghost"
                    className="inline-flex items-center gap-2 !text-status-rework border border-status-rework"
                    style={{ borderColor: "var(--color-status-rework)" }}
                  >
                    <RefreshCw className="w-4 h-4" />
                    На доработку
                  </Button>
                  <Button
                    onClick={() => handleReview("Reject")}
                    loading={saving}
                    variant="ghost"
                    className="inline-flex items-center gap-2 !text-status-rejected border border-status-rejected"
                    style={{ borderColor: "var(--color-status-rejected)" }}
                  >
                    <XCircle className="w-4 h-4" />
                    Отклонить
                  </Button>
                </div>
              )}

              {isDean && (
                <div className="flex flex-wrap gap-3">
                  <Button
                    onClick={() => handleReview("Approve")}
                    loading={saving}
                    className="inline-flex items-center gap-2"
                  >
                    <CheckCircle className="w-4 h-4" />
                    Утвердить
                  </Button>
                  <Button
                    onClick={() => handleReview("Reject")}
                    loading={saving}
                    variant="ghost"
                    className="inline-flex items-center gap-2 !text-status-rejected border border-status-rejected"
                    style={{ borderColor: "var(--color-status-rejected)" }}
                  >
                    <XCircle className="w-4 h-4" />
                    Отклонить
                  </Button>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Info sidebar */}
        <div
          className="bg-card-bg border border-card-border p-6 h-fit space-y-4"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary">
            Информация
          </h2>

          <div>
            <p className="text-xs text-text-muted">Текущий статус</p>
            <p
              className="text-sm font-medium mt-0.5"
              style={{ color: getStatusColor(doc.currentStatusName) }}
            >
              {doc.currentStatusName}
            </p>
          </div>

          <div>
            <p className="text-xs text-text-muted">Автор</p>
            <p className="text-sm font-medium mt-0.5">{doc.authorName}</p>
          </div>

          <div>
            <p className="text-xs text-text-muted">Дата отправки</p>
            <p className="text-sm font-medium mt-0.5">
              {doc.sendToReviewAt
                ? new Date(doc.sendToReviewAt).toLocaleDateString("ru-RU")
                : "—"}
            </p>
          </div>

          <div>
            <p className="text-xs text-text-muted">Вложений</p>
            <p className="text-sm font-medium mt-0.5">
              {doc.attachments.length}
            </p>
          </div>

          <Link
            href={`/documents/${id}`}
            className="block text-sm text-primary hover:underline"
          >
            ← Назад к документу
          </Link>
        </div>
      </div>
    </div>
  );
}

function InfoField({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <p className="text-xs text-text-muted">{label}</p>
      <p className="text-sm font-medium text-text-primary">{value}</p>
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
