"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { createUser } from "@/lib/admin";
import {
  getDepartments,
  getAcademicGroups,
  getStudentStatuses,
} from "@/lib/references";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import type {
  SystemRoleName,
  Sex,
  DepartmentDto,
  AcademicGroupDto,
  StudentStatusDto,
} from "@/types";
import { ROLE_LABELS } from "@/types";
import { ArrowLeft } from "lucide-react";

const roles: { value: SystemRoleName; label: string }[] = [
  { value: "StudentProfile", label: ROLE_LABELS.StudentProfile },
  { value: "StaffProfile", label: ROLE_LABELS.StaffProfile },
  { value: "Dean", label: ROLE_LABELS.Dean },
  { value: "Secretary", label: ROLE_LABELS.Secretary },
  { value: "Curator", label: ROLE_LABELS.Curator },
  { value: "Admin", label: ROLE_LABELS.Admin },
];

export default function NewUserPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [patronymic, setPatronymic] = useState("");
  const [sex, setSex] = useState<Sex>("Male");
  const [role, setRole] = useState<SystemRoleName>("StudentProfile");

  const [studentTicket, setStudentTicket] = useState("");
  const [academicGroupId, setAcademicGroupId] = useState("");
  const [studentStatusId, setStudentStatusId] = useState("");
  const [departmentId, setDepartmentId] = useState("");

  const [departments, setDepartments] = useState<DepartmentDto[]>([]);
  const [groups, setGroups] = useState<AcademicGroupDto[]>([]);
  const [statuses, setStudentStatuses] = useState<StudentStatusDto[]>([]);
  const [refsLoading, setRefsLoading] = useState(true);

  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  const isStudent = role === "StudentProfile";
  const needsProfile = role !== "Admin";

  useEffect(() => {
    const load = async () => {
      try {
        const [d, g, s] = await Promise.all([
          getDepartments(),
          getAcademicGroups(),
          getStudentStatuses(),
        ]);
        setDepartments(d);
        setGroups(g);
        setStudentStatuses(s);
      } catch {
        // silent
      } finally {
        setRefsLoading(false);
      }
    };
    load();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (!email.trim() || !password || !firstName.trim() || !lastName.trim()) {
      setError("Заполните обязательные поля");
      return;
    }
    if (password.length < 6) {
      setError("Пароль должен быть не менее 6 символов");
      return;
    }

    setSaving(true);
    try {
      await createUser({
        email: email.trim(),
        password,
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        patronymic: patronymic.trim() || null,
        sex,
        role,
        studentTicket: isStudent ? studentTicket.trim() || null : null,
        academicGroupId: isStudent ? (academicGroupId ? Number(academicGroupId) : null) : null,
        studentStatusId: isStudent ? (studentStatusId ? Number(studentStatusId) : null) : null,
        departmentId: !isStudent && needsProfile ? (departmentId ? Number(departmentId) : null) : null,
      });
      router.push("/admin/users");
    } catch {
      setError("Ошибка при создании пользователя");
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <div className="flex items-center gap-4">
        <button
          onClick={() => router.push("/admin/users")}
          className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
        >
          <ArrowLeft className="w-5 h-5 text-text-secondary" />
        </button>
        <h1 className="text-2xl font-bold text-text-primary">
          Создать пользователя
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
            <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin" />
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-5">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              <Input
                label="Email *"
                name="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="email@uni.ru"
              />
              <Input
                label="Пароль *"
                name="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="Минимум 6 символов"
              />
              <Input
                label="Фамилия *"
                name="lastName"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                placeholder="Иванов"
              />
              <Input
                label="Имя *"
                name="firstName"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                placeholder="Иван"
              />
              <Input
                label="Отчество"
                name="patronymic"
                value={patronymic}
                onChange={(e) => setPatronymic(e.target.value)}
                placeholder="Иванович"
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
              <div className="flex flex-col gap-1.5">
                <label className="text-sm text-text-secondary">Пол</label>
                <select
                  value={sex}
                  onChange={(e) => setSex(e.target.value as Sex)}
                  className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                >
                  <option value="Male">Мужской</option>
                  <option value="Female">Женский</option>
                  <option value="Other">Другой</option>
                </select>
              </div>

              <div className="flex flex-col gap-1.5">
                <label className="text-sm text-text-secondary">Роль</label>
                <select
                  value={role}
                  onChange={(e) =>
                    setRole(e.target.value as SystemRoleName)
                  }
                  className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                >
                  {roles.map((r) => (
                    <option key={r.value} value={r.value}>
                      {r.label}
                    </option>
                  ))}
                </select>
              </div>
            </div>

            {isStudent && (
              <div className="border-t border-card-border pt-5 space-y-5">
                <h3 className="font-semibold text-text-primary">
                  Профиль студента
                </h3>
                <Input
                  label="Номер студенческого"
                  name="studentTicket"
                  value={studentTicket}
                  onChange={(e) => setStudentTicket(e.target.value)}
                  placeholder="123456"
                />
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                  <div className="flex flex-col gap-1.5">
                    <label className="text-sm text-text-secondary">
                      Группа
                    </label>
                    <select
                      value={academicGroupId}
                      onChange={(e) =>
                        setAcademicGroupId(e.target.value)
                      }
                      className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                    >
                      <option value="">Не выбрано</option>
                      {groups.map((g) => (
                        <option key={g.id} value={g.id}>
                          {g.name}
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
                      onChange={(e) =>
                        setStudentStatusId(e.target.value)
                      }
                      className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                    >
                      <option value="">Не выбрано</option>
                      {statuses.map((s) => (
                        <option key={s.id} value={s.id}>
                          {s.name}
                        </option>
                      ))}
                    </select>
                  </div>
                </div>
              </div>
            )}

            {!isStudent && needsProfile && (
              <div className="border-t border-card-border pt-5 space-y-5">
                <h3 className="font-semibold text-text-primary">
                  Профиль сотрудника
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                  <div className="flex flex-col gap-1.5">
                    <label className="text-sm text-text-secondary">
                      Кафедра
                    </label>
                    <select
                      value={departmentId}
                      onChange={(e) =>
                        setDepartmentId(e.target.value)
                      }
                      className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
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
                      Группа (кураторство)
                    </label>
                    <select
                      value={academicGroupId}
                      onChange={(e) =>
                        setAcademicGroupId(e.target.value)
                      }
                      className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
                    >
                      <option value="">Не выбрано</option>
                      {groups.map((g) => (
                        <option key={g.id} value={g.id}>
                          {g.name}
                        </option>
                      ))}
                    </select>
                  </div>
                </div>
              </div>
            )}

            {error && (
              <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                {error}
              </div>
            )}

            <div className="flex gap-3">
              <Button type="submit" loading={saving}>
                Создать
              </Button>
              <Button
                type="button"
                variant="ghost"
                onClick={() => router.push("/admin/users")}
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
