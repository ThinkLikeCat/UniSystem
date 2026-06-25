"use client";

import { useEffect, useState } from "react";
import {
  getDocumentTypes,
  createDocumentType,
  updateDocumentType,
  deleteDocumentType,
} from "@/lib/references";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import type { DocumentTypeDto } from "@/types";
import { Plus, Search, Pencil, Trash2, BookType } from "lucide-react";

export default function DocumentTypesPage() {
  const [items, setItems] = useState<DocumentTypeDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editItem, setEditItem] = useState<DocumentTypeDto | null>(null);
  const [name, setName] = useState("");
  const [requiresAttachments, setRequiresAttachments] = useState(false);
  const [templateText, setTemplateText] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const fetchItems = async () => {
    setLoading(true);
    try {
      const data = await getDocumentTypes();
      setItems(data);
    } catch {
      // silent
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchItems();
  }, []);

  const openCreate = () => {
    setEditItem(null);
    setName("");
    setRequiresAttachments(false);
    setTemplateText("");
    setError("");
    setModalOpen(true);
  };

  const openEdit = (item: DocumentTypeDto) => {
    setEditItem(item);
    setName(item.name);
    setRequiresAttachments(item.requiresAttachments);
    setTemplateText(item.templateText);
    setError("");
    setModalOpen(true);
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) {
      setError("Название обязательно");
      return;
    }
    setSaving(true);
    try {
      const data = { name: name.trim(), requiresAttachments, templateText: templateText.trim() };
      if (editItem) {
        await updateDocumentType(editItem.id, data);
      } else {
        await createDocumentType(data);
      }
      setModalOpen(false);
      await fetchItems();
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (item: DocumentTypeDto) => {
    if (!confirm(`Удалить тип документа "${item.name}"?`)) return;
    try {
      await deleteDocumentType(item.id);
      await fetchItems();
    } catch {
      alert("Ошибка при удалении");
    }
  };

  const filtered = items.filter((i) =>
    i.name.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="p-6 max-w-5xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">
            Типы документов
          </h1>
          <p className="text-sm text-text-secondary mt-1">
            Всего: {items.length}
          </p>
        </div>
        <Button onClick={openCreate}>
          <Plus className="w-5 h-5" />
          Добавить
        </Button>
      </div>

      <div
        className="bg-card-bg border border-card-border p-4"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-text-muted" />
          <input
            type="text"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            placeholder="Поиск по названию..."
            className="w-full h-10 pl-9 pr-4 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all"
          />
        </div>
      </div>

      <div
        className="bg-card-bg border border-card-border"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        {loading ? (
          <div className="flex justify-center py-12">
            <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
          </div>
        ) : filtered.length === 0 ? (
          <div className="flex flex-col items-center py-12 text-text-muted">
            <BookType className="w-12 h-12 mb-3 opacity-50" />
            <p>Типы документов не найдены</p>
          </div>
        ) : (
          <>
            <div className="hidden md:grid grid-cols-12 gap-4 px-6 py-3 border-b border-card-border text-xs font-medium text-text-muted uppercase tracking-wider">
              <div className="col-span-4">Название</div>
              <div className="col-span-2">Вложения</div>
              <div className="col-span-3">Шаблон</div>
              <div className="col-span-3">Действия</div>
            </div>
            <div className="divide-y divide-card-border">
              {filtered.map((item) => (
                <div
                  key={item.id}
                  className="grid grid-cols-1 md:grid-cols-12 gap-2 md:gap-4 px-6 py-4 items-center"
                >
                  <div className="md:col-span-4 text-sm text-text-primary font-medium">
                    {item.name}
                  </div>
                  <div className="md:col-span-2">
                    <span
                      className={`text-xs font-medium px-2 py-0.5 rounded-full ${
                        item.requiresAttachments
                          ? "bg-warning/10 text-warning"
                          : "bg-text-muted/10 text-text-muted"
                      }`}
                    >
                      {item.requiresAttachments ? "Да" : "Нет"}
                    </span>
                  </div>
                  <div className="md:col-span-3 text-xs text-text-muted truncate">
                    {item.templateText
                      ? item.templateText.substring(0, 50) + "..."
                      : "—"}
                  </div>
                  <div className="md:col-span-3 flex items-center gap-2">
                    <button
                      onClick={() => openEdit(item)}
                      className="px-3 py-1.5 text-xs font-medium text-primary hover:bg-primary/5 rounded-[8px] transition-colors"
                    >
                      <Pencil className="w-4 h-4 inline mr-1" />
                      Редактировать
                    </button>
                    <button
                      onClick={() => handleDelete(item)}
                      className="p-1.5 hover:bg-error-bg rounded-[8px] transition-colors"
                    >
                      <Trash2 className="w-4 h-4 text-error-text" />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          </>
        )}
      </div>

      {modalOpen && (
        <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-50">
          <div
            className="bg-card-bg border border-card-border p-6 w-full max-w-lg mx-4"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="text-lg font-semibold text-text-primary mb-4">
              {editItem
                ? "Редактировать тип документа"
                : "Добавить тип документа"}
            </h2>
            <form onSubmit={handleSave} className="space-y-4">
              <Input
                label="Название"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Название типа"
                autoFocus
              />
              <div className="flex items-center gap-3">
                <input
                  type="checkbox"
                  id="requiresAttachments"
                  checked={requiresAttachments}
                  onChange={(e) => setRequiresAttachments(e.target.checked)}
                  className="w-4 h-4 text-primary border-input-border rounded focus:ring-primary"
                />
                <label
                  htmlFor="requiresAttachments"
                  className="text-sm text-text-secondary"
                >
                  Требует вложений
                </label>
              </div>
              <div className="flex flex-col gap-1.5">
                <label className="text-sm text-text-secondary">
                  Текст шаблона
                </label>
                <textarea
                  value={templateText}
                  onChange={(e) => setTemplateText(e.target.value)}
                  placeholder="Шаблон с плейсхолдерами {student_name}, {group_name}, ..."
                  rows={4}
                  className="w-full px-4 py-3 bg-input-bg border border-input-border rounded-[16px] text-sm text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all resize-none"
                />
              </div>
              {error && (
                <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                  {error}
                </div>
              )}
              <div className="flex gap-3">
                <Button type="submit" loading={saving}>
                  Сохранить
                </Button>
                <Button
                  type="button"
                  variant="ghost"
                  onClick={() => setModalOpen(false)}
                >
                  Отмена
                </Button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
