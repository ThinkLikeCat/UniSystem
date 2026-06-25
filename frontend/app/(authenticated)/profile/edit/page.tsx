"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "@/components/AuthProvider";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import { updateStudentProfile, updateStaffProfile } from "@/lib/profile";
import { getAcademicGroups, getStudentStatuses, getDepartments } from "@/lib/references";
import type {
  AcademicGroupDto,
  StudentStatusDto,
  DepartmentDto,
} from "@/types";
import { ArrowLeft, Loader2 } from "lucide-react";

export default function ProfileEditPage() {
  const { user } = useAuth();
  const router = useRouter();

  // references
  const [groups, setGroups] = useState<AcademicGroupDto[]>([]);
  const [statuses, setStatuses] = useState<StudentStatusDto[]>([]);
  const [departments, setDepartments] = useState<DepartmentDto[]>([]);
  const [refsLoading, setRefsLoading] = useState(true);

  // form state
  const [studentTicket, setStudentTicket] = useState("");
  const [academicGroupId, setAcademicGroupId] = useState("");
  const [studentStatusId, setStudentStatusId] = useState("");
  const [departmentId, setDepartmentId] = useState("");

  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const isStudent = user?.role === "StudentProfile";

  useEffect(() => {
    const load = async () => {
      try {
        const [g, s, d] = await Promise.all([
          getAcademicGroups(),
          getStudentStatuses(),
          getDepartments(),
        ]);
        setGroups(g);
        setStatuses(s);
        setDepartments(d);
      } catch {
        // silent
      } finally {
        setRefsLoading(false);
      }
    };
    load();
  }, []);

  useEffect(() => {
    if (!user) return;
    if (isStudent && user.studentProfile) {
      setStudentTicket(user.studentProfile.studentTicket || "");
      setAcademicGroupId(String(user.studentProfile.academicGroupId || ""));
      setStudentStatusId(String(user.studentProfile.studentStatusId || ""));
    }
    if (!isStudent && user.staffProfile) {
      setDepartmentId(String(user.staffProfile.departmentId || ""));
      setAcademicGroupId(String(user.staffProfile.academicGroupId || ""));
    }
  }, [user, isStudent]);

  if (!user) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setSaving(true);

    try {
      if (isStudent) {
        await updateStudentProfile({
          studentTicket: studentTicket || null,
          academicGroupId: academicGroupId ? Number(academicGroupId) : null,
          studentStatusId: studentStatusId ? Number(studentStatusId) : null,
        });
      } else {
        await updateStaffProfile({
          departmentId: departmentId ? Number(departmentId) : null,
          academicGroupId: academicGroupId ? Number(academicGroupId) : null,
        });
      }
      setSuccess("Профиль обновлён");
      setTimeout(() => router.push("/profile"), 1500);
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="p-6 max-w-2xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push("/profile")}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <h1 className="text-2xl font-bold text-text-primary">
          Редактировать профиль
        </h1>
      </div>

      <div
        className="bg-card-bg border border-card-border p-6"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        {refsLoading ? (
          <div className="flex justify-center py-8">
            <Loader2 className="w-6 h-6 animate-spin text-primary" />
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-5">
            {isStudent ? (
              <>
                <Input
                  label="Номер студенческого"
                  name="studentTicket"
                  value={studentTicket}
                  onChange={(e) => setStudentTicket(e.target.value)}
                  placeholder="123456"
                />

                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">
                    Группа
                  </label>
                  <select
                    value={academicGroupId}
                    onChange={(e) => setAcademicGroupId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                  >
                    <option value="">Не выбрано</option>
                    {groups.map((g) => (
                      <option key={g.id} value={g.id}>
                        {g.name} ({g.specialtyName})
                      </option>
                    ))}
                  </select>
                </div>

                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">
                    Статус
                  </label>
                  <select
                    value={studentStatusId}
                    onChange={(e) => setStudentStatusId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                  >
                    <option value="">Не выбрано</option>
                    {statuses.map((s) => (
                      <option key={s.id} value={s.id}>
                        {s.name}
                      </option>
                    ))}
                  </select>
                </div>
              </>
            ) : (
              <>
                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">
                    Кафедра
                  </label>
                  <select
                    value={departmentId}
                    onChange={(e) => setDepartmentId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                  >
                    <option value="">Не выбрано</option>
                    {departments.map((d) => (
                      <option key={d.id} value={d.id}>
                        {d.name}
                      </option>
                    ))}
                  </select>
                </div>

                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">
                    Группа
                  </label>
                  <select
                    value={academicGroupId}
                    onChange={(e) => setAcademicGroupId(e.target.value)}
                    className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all duration-200"
                  >
                    <option value="">Не выбрано</option>
                    {groups.map((g) => (
                      <option key={g.id} value={g.id}>
                        {g.name} ({g.specialtyName})
                      </option>
                    ))}
                  </select>
                </div>
              </>
            )}

            {error && (
              <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                {error}
              </div>
            )}

            {success && (
              <div className="bg-green-50 border border-green-200 rounded-[12px] px-4 py-3 text-sm text-green-700">
                {success}
              </div>
            )}

            <div className="flex gap-3">
              <Button type="submit" loading={saving}>
                Сохранить
              </Button>
              <Button
                type="button"
                variant="ghost"
                onClick={() => router.push("/profile")}
              >
                Отмена
              </Button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
}
