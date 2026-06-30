"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { useAuth } from "@/components/AuthProvider";
import {
  getDocumentById,
  getDocumentTypeTemplate,
  updateDocumentDynamicValues,
} from "@/lib/documents";
import { Button } from "@/components/ui/Button";
import type { DocumentDetailDto, TemplatePreviewDto } from "@/types";
import { ArrowLeft, Info } from "lucide-react";

const userFieldLabels: Record<string, string> = {
  reason: "Причина",
  subject: "Предмет",
  faculty: "Факультет",
  single_date: "Дата",
  start_date: "Дата начала",
  end_date: "Дата окончания",
  student_name: "Студент",
  group_name: "Группа",
  course: "Курс",
  specialty: "Специальность",
};

export default function EditDocumentPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();
  const { user } = useAuth();
  const [doc, setDoc] = useState<DocumentDetailDto | null>(null);
  const [template, setTemplate] = useState<TemplatePreviewDto | null>(null);
  const [dynamicValues, setDynamicValues] = useState<Record<string, string>>(
    {}
  );
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const load = async () => {
      try {
        const d = await getDocumentById(id);
        setDoc(d);

        const parsed: Record<string, string> = {};
        if (d.dynamicValues) {
          try {
            Object.assign(parsed, JSON.parse(d.dynamicValues));
          } catch {
            // ignore
          }
        }
        setDynamicValues(parsed);

        const tpl = await getDocumentTypeTemplate(d.documentTypeId);
        setTemplate(tpl);
      } catch {
        setError("Ошибка загрузки документа");
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [id]);

  if (!user) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError("");

    try {
      await updateDocumentDynamicValues(id, {
        documentId: id,
        dynamicValues: JSON.stringify(dynamicValues),
      });
      router.push(`/documents/${id}`);
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  const allFields = template?.userFields ?? [];

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

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push(`/documents/${id}`)}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <div>
          <h1 className="text-2xl font-bold text-text-primary">
            Редактировать документ
          </h1>
          <p className="text-sm text-text-secondary mt-1">
            {doc.documentTypeName}
          </p>
        </div>
      </div>

      {template && template.systemFields && Object.keys(template.systemFields).length > 0 && (
        <div
          className="bg-card-bg border border-card-border p-4"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <div className="flex items-center gap-2 mb-3">
            <Info className="w-4 h-4 text-text-muted" />
            <span className="text-sm font-medium text-text-primary">
              Системные поля (заполняются автоматически)
            </span>
          </div>
          <div className="grid grid-cols-2 gap-3">
            {Object.entries(template.systemFields).map(([key, value]) => (
              <div key={key}>
                <span className="text-xs text-text-muted">
                  {userFieldLabels[key] || key}:
                </span>
                <span className="text-sm text-text-primary ml-2 font-medium">
                  {value}
                </span>
              </div>
            ))}
          </div>
        </div>
      )}

      <div
        className="bg-card-bg border border-card-border p-6"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        <form onSubmit={handleSubmit} className="space-y-5">
          <h2 className="font-semibold text-text-primary">
            Поля документа
          </h2>

          <div className="space-y-4">
            {allFields.length > 0 ? (
              allFields.map((field) => (
                <div key={field}>
                  {field === "reason" ? (
                    <div className="space-y-1.5">
                      <label className="text-sm text-text-secondary">
                        {userFieldLabels[field] || field}
                      </label>
                      <textarea
                        value={dynamicValues[field] ?? ""}
                        onChange={(e) =>
                          setDynamicValues((prev) => ({
                            ...prev,
                            [field]: e.target.value,
                          }))
                        }
                        rows={3}
                        className="w-full px-4 py-3 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200 resize-none"
                      />
                    </div>
                  ) : (
                    <div className="space-y-1.5">
                      <label className="text-sm text-text-secondary">
                        {userFieldLabels[field] || field}
                      </label>
                      <input
                        type={
                          field.includes("date") ? "date" : "text"
                        }
                        value={dynamicValues[field] ?? ""}
                        onChange={(e) =>
                          setDynamicValues((prev) => ({
                            ...prev,
                            [field]: e.target.value,
                          }))
                        }
                        placeholder={
                          userFieldLabels[field] || field
                        }
                        className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                      />
                    </div>
                  )}
                </div>
              ))
            ) : (
              <>
                {Object.entries(dynamicValues).map(([key, value]) => (
                  <div key={key} className="flex items-start gap-3">
                    <div className="flex-1 space-y-1.5">
                      <label className="text-sm text-text-secondary">
                        {userFieldLabels[key] || key}
                      </label>
                      {key === "reason" ? (
                        <textarea
                          value={value}
                          onChange={(e) =>
                            setDynamicValues((prev) => ({
                              ...prev,
                              [key]: e.target.value,
                            }))
                          }
                          rows={3}
                          className="w-full px-4 py-3 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200 resize-none"
                        />
                      ) : (
                        <input
                          type={
                            key.includes("date") ? "date" : "text"
                          }
                          value={value}
                          onChange={(e) =>
                            setDynamicValues((prev) => ({
                              ...prev,
                              [key]: e.target.value,
                            }))
                          }
                          className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                        />
                      )}
                    </div>
                  </div>
                ))}

                {Object.keys(dynamicValues).length === 0 && (
                  <p className="text-sm text-text-muted text-center py-4">
                    Нет полей для редактирования
                  </p>
                )}
              </>
            )}
          </div>

          {error && (
            <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
              {error}
            </div>
          )}

          <div className="flex gap-3 pt-2">
            <Button type="submit" loading={saving}>
              Сохранить
            </Button>
            <Button
              type="button"
              variant="ghost"
              onClick={() => router.push(`/documents/${id}`)}
            >
              Отмена
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
