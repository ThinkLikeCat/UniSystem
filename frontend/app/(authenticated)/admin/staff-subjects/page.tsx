"use client";

import { useEffect, useState } from "react";
import { getUsers } from "@/lib/admin";
import { getSubjects } from "@/lib/references";
import {
  getStaffSubjects,
  createStaffSubject,
  deleteStaffSubject,
} from "@/lib/staffSubjects";
import { Button } from "@/components/ui/Button";
import type { UserListItemDto, SubjectDto, StaffSubjectDto } from "@/types";
import { BookOpen, Plus, Trash2, Search } from "lucide-react";

const STAFF_ROLES = ["StaffProfile", "Dean", "Secretary", "Curator"];

export default function StaffSubjectsPage() {
  const [staffList, setStaffList] = useState<UserListItemDto[]>([]);
  const [subjects, setSubjects] = useState<SubjectDto[]>([]);
  const [staffSubjects, setStaffSubjects] = useState<StaffSubjectDto[]>([]);
  const [selectedStaffId, setSelectedStaffId] = useState("");
  const [loading, setLoading] = useState(true);
  const [subjectsLoading, setSubjectsLoading] = useState(false);
  const [selectedSubjectId, setSelectedSubjectId] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [staffSearch, setStaffSearch] = useState("");

  const fetchInitial = async () => {
    setLoading(true);
    try {
      const [users, subs] = await Promise.all([
        getUsers(),
        getSubjects(),
      ]);
      setStaffList(
        users.filter((u) => STAFF_ROLES.includes(u.role))
      );
      setSubjects(subs);
    } catch {
      // silent
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchInitial();
  }, []);

  const fetchStaffSubjects = async (staffId: string) => {
    setSubjectsLoading(true);
    try {
      const data = await getStaffSubjects(staffId);
      setStaffSubjects(data);
    } catch {
      setStaffSubjects([]);
    } finally {
      setSubjectsLoading(false);
    }
  };

  useEffect(() => {
    if (selectedStaffId) {
      fetchStaffSubjects(selectedStaffId);
      setSelectedSubjectId("");
      setError("");
    } else {
      setStaffSubjects([]);
    }
  }, [selectedStaffId]);

  const handleAddSubject = async () => {
    if (!selectedSubjectId || !selectedStaffId) return;
    setSaving(true);
    setError("");
    try {
      await createStaffSubject(
        selectedStaffId,
        parseInt(selectedSubjectId, 10)
      );
      setSelectedSubjectId("");
      await fetchStaffSubjects(selectedStaffId);
    } catch {
      setError("Ошибка при добавлении предмета");
    } finally {
      setSaving(false);
    }
  };

  const handleDeleteSubject = async (subjectId: number) => {
    if (!selectedStaffId) return;
    const subjectName =
      staffSubjects.find((s) => s.subjectId === subjectId)?.subjectName ??
      "";
    if (!confirm(`Удалить предмет "${subjectName}"?`)) return;
    try {
      await deleteStaffSubject(selectedStaffId, subjectId);
      await fetchStaffSubjects(selectedStaffId);
    } catch {
      alert("Ошибка при удалении");
    }
  };

  const selectedStaff = staffList.find((s) => s.id === selectedStaffId);

  const availableSubjects = subjects.filter(
    (s) => !staffSubjects.some((ss) => ss.subjectId === s.id)
  );

  const filteredStaff = staffList.filter((s) => {
    if (!staffSearch) return true;
    const q = staffSearch.toLowerCase();
    return s.fullName.toLowerCase().includes(q) || s.email.toLowerCase().includes(q);
  });

  return (
    <div className="p-6 max-w-5xl mx-auto space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-text-primary">
          Предметы сотрудников
        </h1>
        <p className="text-sm text-text-secondary mt-1">
          Управление привязкой предметов к сотрудникам
        </p>
      </div>

      <div
        className="bg-card-bg border border-card-border p-6"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        <div className="flex flex-col sm:flex-row gap-4 items-end">
          <div className="flex-1 w-full">
            <label className="text-sm text-text-secondary block mb-1.5">
              Сотрудник
            </label>
            <div className="relative">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-text-muted" />
              <input
                type="text"
                value={staffSearch}
                onChange={(e) => setStaffSearch(e.target.value)}
                placeholder="Поиск сотрудника..."
                className="w-full h-12 pl-9 pr-4 bg-input-bg border border-input-border rounded-[16px] text-sm text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all"
              />
            </div>
          </div>
          <div className="w-full sm:w-64">
            <label className="text-sm text-text-secondary block mb-1.5">
              &nbsp;
            </label>
            <select
              value={selectedStaffId}
              onChange={(e) => setSelectedStaffId(e.target.value)}
              className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
            >
              <option value="">Выберите сотрудника</option>
              {filteredStaff.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.fullName}
                </option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {loading ? (
        <div className="flex justify-center py-12">
          <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
        </div>
      ) : !selectedStaffId ? (
        <div className="flex flex-col items-center py-12 text-text-muted">
          <BookOpen className="w-12 h-12 mb-3 opacity-50" />
          <p>Выберите сотрудника для просмотра предметов</p>
        </div>
      ) : (
        <>
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Предметы: {selectedStaff?.fullName}
            </h2>

            {subjectsLoading ? (
              <div className="flex justify-center py-8">
                <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin" />
              </div>
            ) : staffSubjects.length === 0 ? (
              <div className="text-center py-8 text-text-muted text-sm">
                <p>Нет привязанных предметов</p>
              </div>
            ) : (
              <div className="divide-y divide-card-border">
                {staffSubjects.map((ss) => (
                  <div
                    key={ss.subjectId}
                    className="flex items-center justify-between py-3"
                  >
                    <span className="text-sm text-text-primary">
                      {ss.subjectName}
                    </span>
                    <button
                      onClick={() => handleDeleteSubject(ss.subjectId)}
                      className="p-1.5 hover:bg-error-bg rounded-[8px] transition-colors"
                    >
                      <Trash2 className="w-4 h-4 text-error-text" />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>

          {availableSubjects.length > 0 && (
            <div
              className="bg-card-bg border border-card-border p-6"
              style={{
                borderRadius: "var(--radius-card)",
                boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
              }}
            >
              <h3 className="font-semibold text-text-primary mb-4">
                Добавить предмет
              </h3>
              <div className="flex gap-3 items-end">
                <div className="flex-1">
                  <select
                    value={selectedSubjectId}
                    onChange={(e) => setSelectedSubjectId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                  >
                    <option value="">Выберите предмет</option>
                    {availableSubjects.map((s) => (
                      <option key={s.id} value={s.id}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </div>
                <Button
                  onClick={handleAddSubject}
                  disabled={!selectedSubjectId}
                  loading={saving}
                >
                  <Plus className="w-5 h-5" />
                  Добавить
                </Button>
              </div>
              {error && (
                <div className="mt-3 bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                  {error}
                </div>
              )}
            </div>
          )}
        </>
      )}
    </div>
  );
}
