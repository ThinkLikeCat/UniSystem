"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { getDocumentTypeTemplate, createDocument } from "@/lib/documents";
import { getDocumentTypes } from "@/lib/references";
import { Button } from "@/components/ui/Button";
import type { DocumentTypeDto, TemplatePreviewDto } from "@/types";
import { ArrowLeft, FileText, Info } from "lucide-react";

const userFieldLabels: Record<string, string> = {
  reason: "Причина",
  subject: "Предмет",
  faculty: "Факультет",
  single_date: "Дата",
  start_date: "Дата начала",
  end_date: "Дата окончания",
};

export default function NewDocumentPage() {
  const router = useRouter();
  const [docTypes, setDocTypes] = useState<DocumentTypeDto[]>([]);
  const [selectedTypeId, setSelectedTypeId] = useState("");
  const [template, setTemplate] = useState<TemplatePreviewDto | null>(null);
  const [dynamicValues, setDynamicValues] = useState<Record<string, string>>(
    {}
  );
  const [loadingTypes, setLoadingTypes] = useState(true);
  const [loadingTemplate, setLoadingTemplate] = useState(false);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const load = async () => {
      try {
        const types = await getDocumentTypes();
        setDocTypes(types);
      } catch {
        setError("Ошибка загрузки типов документов");
      } finally {
        setLoadingTypes(false);
      }
    };
    load();
  }, []);

  useEffect(() => {
    if (!selectedTypeId) {
      setTemplate(null);
      setDynamicValues({});
      return;
    }

    const load = async () => {
      setLoadingTemplate(true);
      setError("");
      try {
        const tpl = await getDocumentTypeTemplate(Number(selectedTypeId));
        setTemplate(tpl);
        const initial: Record<string, string> = {};
        tpl.userFields.forEach((f) => {
          initial[f] = "";
        });
        setDynamicValues(initial);
      } catch {
        setError("Ошибка загрузки шаблона");
      } finally {
        setLoadingTemplate(false);
      }
    };
    load();
  }, [selectedTypeId]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedTypeId) return;

    const hasEmpty = template?.userFields.some(
      (f) => !dynamicValues[f]?.trim()
    );
    if (hasEmpty) {
      setError("Заполните все поля");
      return;
    }

    setSaving(true);
    setError("");

    try {
      const docId = await createDocument({
        documentTypeId: Number(selectedTypeId),
        dynamicValues:
          Object.keys(dynamicValues).length > 0
            ? JSON.stringify(dynamicValues)
            : null,
      });
      router.push(`/documents/${docId}`);
    } catch {
      setError("Ошибка при создании документа");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push("/documents")}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <h1 className="text-2xl font-bold text-text-primary">
          Создать документ
        </h1>
      </div>

      <div
        className="bg-card-bg border border-card-border p-6"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        {loadingTypes ? (
          <div className="flex justify-center py-8">
            <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin" />
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-5">
            <div className="flex flex-col gap-1.5">
              <label className="text-sm text-text-secondary">
                Тип документа
              </label>
              <select
                value={selectedTypeId}
                onChange={(e) => setSelectedTypeId(e.target.value)}
                className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
              >
                <option value="">Выберите тип документа</option>
                {docTypes.map((t) => (
                  <option key={t.id} value={t.id}>
                    {t.name}
                  </option>
                ))}
              </select>
            </div>

            {loadingTemplate && (
              <div className="flex justify-center py-4">
                <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin" />
              </div>
            )}

            {template && !loadingTemplate && (
              <>
                <div className="bg-hero-bg rounded-[12px] p-4 space-y-2">
                  <div className="flex items-center gap-2 text-sm font-medium text-text-primary">
                    <Info className="w-4 h-4" />
                    Системные поля (заполнятся автоматически)
                  </div>
                  <div className="grid grid-cols-2 gap-3">
                    {Object.entries(template.systemFields).map(
                      ([key, value]) => (
                        <div key={key} className="text-sm">
                          <span className="text-text-muted">
                            {getFieldLabel(key)}
                            :{" "}
                          </span>
                          <span className="text-text-primary font-medium">
                            {value}
                          </span>
                        </div>
                      )
                    )}
                  </div>
                </div>

                <div className="border-t border-card-border pt-4">
                  <p className="text-sm font-medium text-text-primary mb-4">
                    Заполните поля документа
                  </p>
                  <div className="space-y-4">
                    {template.userFields.map((field) => (
                      <div key={field} className="flex flex-col gap-1.5">
                        <label className="text-sm text-text-secondary">
                          {userFieldLabels[field] || field}
                        </label>
                        {field === "reason" ? (
                          <textarea
                            value={dynamicValues[field] || ""}
                            onChange={(e) =>
                              setDynamicValues((prev) => ({
                                ...prev,
                                [field]: e.target.value,
                              }))
                            }
                            rows={3}
                            className="w-full px-4 py-3 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200 resize-none"
                            placeholder={`Введите ${userFieldLabels[field]?.toLowerCase() || field}`}
                          />
                        ) : (
                          <input
                            type={
                              field.includes("date")
                                ? "date"
                                : "text"
                            }
                            value={dynamicValues[field] || ""}
                            onChange={(e) =>
                              setDynamicValues((prev) => ({
                                ...prev,
                                [field]: e.target.value,
                              }))
                            }
                            className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                            placeholder={`Введите ${userFieldLabels[field]?.toLowerCase() || field}`}
                          />
                        )}
                      </div>
                    ))}
                  </div>
                </div>

                {template.userFields.length > 0 && (
                  <div
                    className="bg-hero-bg rounded-[12px] p-4"
                    style={{ borderRadius: "var(--radius-card)" }}
                  >
                    <p className="text-sm font-medium text-text-primary mb-2">
                      Предпросмотр шаблона
                    </p>
                    <div className="text-sm text-text-secondary whitespace-pre-wrap">
                      {previewTemplate(
                        template.documentTypeName,
                        template.systemFields,
                        dynamicValues
                      )}
                    </div>
                  </div>
                )}
              </>
            )}

            {error && (
              <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                {error}
              </div>
            )}

            {selectedTypeId && !loadingTemplate && (
              <div className="flex gap-3 pt-2">
                <Button type="submit" loading={saving}>
                  Создать
                </Button>
                <Button
                  type="button"
                  variant="ghost"
                  onClick={() => router.push("/documents")}
                >
                  Отмена
                </Button>
              </div>
            )}
          </form>
        )}
      </div>
    </div>
  );
}

function getFieldLabel(key: string): string {
  const labels: Record<string, string> = {
    student_name: "Студент",
    group_name: "Группа",
    course: "Курс",
    specialty: "Специальность",
  };
  return labels[key] || key;
}

function previewTemplate(
  docTypeName: string,
  systemFields: Record<string, string>,
  userValues: Record<string, string>
): string {
  const lines = [
    `Тип: ${docTypeName}`,
    `Студент: ${systemFields.student_name || "—"}`,
    `Группа: ${systemFields.group_name || "—"}`,
    `Курс: ${systemFields.course || "—"}`,
    `Специальность: ${systemFields.specialty || "—"}`,
  ];
  Object.entries(userValues).forEach(([key, value]) => {
    if (value) {
      lines.push(`${getFieldLabel(key)}: ${value}`);
    }
  });
  return lines.join("\n");
}
