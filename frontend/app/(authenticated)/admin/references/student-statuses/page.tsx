"use client";

import { useEffect, useState } from "react";
import {
  getStudentStatuses,
  createStudentStatus,
  updateStudentStatus,
  deleteStudentStatus,
} from "@/lib/references";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import type { StudentStatusDto } from "@/types";
import { Plus, Search, Pencil, Trash2, FolderOpen } from "lucide-react";

export default function StudentStatusesPage() {
  const [items, setItems] = useState<StudentStatusDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editItem, setEditItem] = useState<StudentStatusDto | null>(null);
  const [name, setName] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const fetchItems = async () => {
    setLoading(true);
    try {
      const data = await getStudentStatuses();
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
    setError("");
    setModalOpen(true);
  };

  const openEdit = (item: StudentStatusDto) => {
    setEditItem(item);
    setName(item.name);
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
      if (editItem) {
        await updateStudentStatus(editItem.id, name.trim());
      } else {
        await createStudentStatus(name.trim());
      }
      setModalOpen(false);
      await fetchItems();
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (item: StudentStatusDto) => {
    if (!confirm(`Удалить статус "${item.name}"?`)) return;
    try {
      await deleteStudentStatus(item.id);
      await fetchItems();
    } catch {
      alert("Ошибка при удалении");
    }
  };

  const filtered = items.filter((i) =>
    i.name.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="p-6 max-w-4xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">
            Статусы студентов
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
            <FolderOpen className="w-12 h-12 mb-3 opacity-50" />
            <p>Статусы не найдены</p>
          </div>
        ) : (
          <>
            <div className="hidden md:grid grid-cols-12 gap-4 px-6 py-3 border-b border-card-border text-xs font-medium text-text-muted uppercase tracking-wider">
              <div className="col-span-8">Название</div>
              <div className="col-span-4">Действия</div>
            </div>
            <div className="divide-y divide-card-border">
              {filtered.map((item) => (
                <div
                  key={item.id}
                  className="grid grid-cols-1 md:grid-cols-12 gap-2 md:gap-4 px-6 py-4 items-center"
                >
                  <div className="md:col-span-8 text-sm text-text-primary font-medium">
                    {item.name}
                  </div>
                  <div className="md:col-span-4 flex items-center gap-2">
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
            className="bg-card-bg border border-card-border p-6 w-full max-w-md mx-4"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="text-lg font-semibold text-text-primary mb-4">
              {editItem
                ? "Редактировать статус"
                : "Добавить статус"}
            </h2>
            <form onSubmit={handleSave} className="space-y-4">
              <Input
                label="Название"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Название статуса"
                autoFocus
              />
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
