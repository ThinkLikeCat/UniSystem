"use client";

import { useEffect, useState } from "react";
import {
  getAcademicGroups,
  createAcademicGroup,
  updateAcademicGroup,
  deleteAcademicGroup,
  getSpecialties,
} from "@/lib/references";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import type { AcademicGroupDto, SpecialtyDto } from "@/types";
import { Plus, Search, Pencil, Trash2, GraduationCap } from "lucide-react";

export default function AcademicGroupsPage() {
  const [items, setItems] = useState<AcademicGroupDto[]>([]);
  const [specialties, setSpecialties] = useState<SpecialtyDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchQuery, setSearchQuery] = useState("");
  const [modalOpen, setModalOpen] = useState(false);
  const [editItem, setEditItem] = useState<AcademicGroupDto | null>(null);
  const [name, setName] = useState("");
  const [maxCount, setMaxCount] = useState("");
  const [course, setCourse] = useState("");
  const [specialtyId, setSpecialtyId] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const fetchData = async () => {
    setLoading(true);
    try {
      const [groups, specs] = await Promise.all([
        getAcademicGroups(),
        getSpecialties(),
      ]);
      setItems(groups);
      setSpecialties(specs);
    } catch {
      // silent
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const openCreate = () => {
    setEditItem(null);
    setName("");
    setMaxCount("");
    setCourse("");
    setSpecialtyId("");
    setError("");
    setModalOpen(true);
  };

  const openEdit = (item: AcademicGroupDto) => {
    setEditItem(item);
    setName(item.name);
    setMaxCount(String(item.maxCount));
    setCourse(String(item.course));
    setSpecialtyId(String(item.specialtyId));
    setError("");
    setModalOpen(true);
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) {
      setError("Название обязательно");
      return;
    }
    const max = parseInt(maxCount, 10);
    const c = parseInt(course, 10);
    const sId = parseInt(specialtyId, 10);
    if (!max || max < 1) {
      setError("Укажите корректный максимальный состав");
      return;
    }
    if (!c || c < 1) {
      setError("Укажите корректный курс");
      return;
    }
    if (!sId) {
      setError("Выберите специальность");
      return;
    }
    setSaving(true);
    try {
      const data = { name: name.trim(), maxCount: max, course: c, specialtyId: sId };
      if (editItem) {
        await updateAcademicGroup(editItem.id, data);
      } else {
        await createAcademicGroup(data);
      }
      setModalOpen(false);
      await fetchData();
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (item: AcademicGroupDto) => {
    if (!confirm(`Удалить группу "${item.name}"?`)) return;
    try {
      await deleteAcademicGroup(item.id);
      await fetchData();
    } catch {
      alert("Ошибка при удалении");
    }
  };

  const filtered = items.filter(
    (i) =>
      i.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
      i.specialtyName.toLowerCase().includes(searchQuery.toLowerCase())
  );

  return (
    <div className="p-6 max-w-5xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">Группы</h1>
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
            placeholder="Поиск по названию или специальности..."
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
            <GraduationCap className="w-12 h-12 mb-3 opacity-50" />
            <p>Группы не найдены</p>
          </div>
        ) : (
          <>
            <div className="hidden md:grid grid-cols-12 gap-4 px-6 py-3 border-b border-card-border text-xs font-medium text-text-muted uppercase tracking-wider">
              <div className="col-span-3">Название</div>
              <div className="col-span-2">Курс</div>
              <div className="col-span-2">Макс.</div>
              <div className="col-span-3">Специальность</div>
              <div className="col-span-2">Действия</div>
            </div>
            <div className="divide-y divide-card-border">
              {filtered.map((item) => (
                <div
                  key={item.id}
                  className="grid grid-cols-1 md:grid-cols-12 gap-2 md:gap-4 px-6 py-4 items-center"
                >
                  <div className="md:col-span-3 text-sm text-text-primary font-medium">
                    {item.name}
                  </div>
                  <div className="md:col-span-2 text-sm text-text-secondary">
                    {item.course}
                  </div>
                  <div className="md:col-span-2 text-sm text-text-secondary">
                    {item.maxCount}
                  </div>
                  <div className="md:col-span-3 text-sm text-text-secondary">
                    {item.specialtyName}
                  </div>
                  <div className="md:col-span-2 flex items-center gap-2">
                    <button
                      onClick={() => openEdit(item)}
                      className="p-1.5 hover:bg-primary/5 rounded-[8px] transition-colors"
                    >
                      <Pencil className="w-4 h-4 text-primary" />
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
              {editItem ? "Редактировать группу" : "Добавить группу"}
            </h2>
            <form onSubmit={handleSave} className="space-y-4">
              <Input
                label="Название"
                name="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="ИС-21"
                autoFocus
              />
              <div className="grid grid-cols-3 gap-4">
                <Input
                  label="Курс"
                  name="course"
                  type="number"
                  value={course}
                  onChange={(e) => setCourse(e.target.value)}
                  placeholder="2"
                />
                <Input
                  label="Макс. состав"
                  name="maxCount"
                  type="number"
                  value={maxCount}
                  onChange={(e) => setMaxCount(e.target.value)}
                  placeholder="30"
                />
                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">
                    Специальность
                  </label>
                  <select
                    value={specialtyId}
                    onChange={(e) => setSpecialtyId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                  >
                    <option value="">Выберите</option>
                    {specialties.map((s) => (
                      <option key={s.id} value={s.id}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </div>
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
