"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { getUserById, updateUser, resetPassword, deleteUser } from "@/lib/admin";
import {
  getDepartments,
  getAcademicGroups,
  getStudentStatuses,
} from "@/lib/references";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import type {
  UserDetailDto,
  Sex,
  SystemRoleName,
  DepartmentDto,
  AcademicGroupDto,
  StudentStatusDto,
} from "@/types";
import { ROLE_LABELS } from "@/types";
import {
  ArrowLeft,
  Trash2,
  Key,
  Save,
  User,
  Mail,
  Shield,
} from "lucide-react";

export default function UserDetailPage() {
  const { id } = useParams<{ id: string }>();
  const router = useRouter();

  const [userData, setUserData] = useState<UserDetailDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  // edit form
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [patronymic, setPatronymic] = useState("");
  const [sex, setSex] = useState<Sex>("Male");
  const [role, setRole] = useState("");

  // profile fields
  const [studentTicket, setStudentTicket] = useState("");
  const [academicGroupId, setAcademicGroupId] = useState("");
  const [studentStatusId, setStudentStatusId] = useState("");
  const [departmentId, setDepartmentId] = useState("");

  const [departments, setDepartments] = useState<DepartmentDto[]>([]);
  const [groups, setGroups] = useState<AcademicGroupDto[]>([]);
  const [statuses, setStudentStatuses] = useState<StudentStatusDto[]>([]);
  const [refsLoading, setRefsLoading] = useState(true);

  // reset password
  const [newPassword, setNewPassword] = useState("");
  const [showReset, setShowReset] = useState(false);

  const fetchUser = async () => {
    try {
      const [u, d, g, s] = await Promise.all([
        getUserById(id),
        getDepartments(),
        getAcademicGroups(),
        getStudentStatuses(),
      ]);
      setUserData(u);
      setFirstName(u.firstName);
      setLastName(u.lastName);
      setPatronymic(u.patronymic || "");
      setSex(u.sex as Sex);
      setRole(u.role);
      setStudentTicket(u.studentTicket || "");
      setAcademicGroupId(u.academicGroupId?.toString() || "");
      setStudentStatusId(u.studentStatusId?.toString() || "");
      setDepartmentId(u.departmentId?.toString() || "");
      setDepartments(d);
      setGroups(g);
      setStudentStatuses(s);
    } catch {
      setError("Ошибка загрузки пользователя");
    } finally {
      setLoading(false);
      setRefsLoading(false);
    }
  };

  useEffect(() => {
    fetchUser();
  }, [id]);

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError("");
    try {
      const isStudent = role === "StudentProfile";
      const needsProfile = role !== "Admin";
      await updateUser(id, {
        firstName,
        lastName,
        patronymic: patronymic || null,
        sex,
        role,
        studentTicket: isStudent ? studentTicket.trim() || null : null,
        academicGroupId: isStudent ? (academicGroupId ? Number(academicGroupId) : null) : needsProfile && role === "Curator" ? (academicGroupId ? Number(academicGroupId) : null) : null,
        studentStatusId: isStudent ? (studentStatusId ? Number(studentStatusId) : null) : null,
        departmentId: needsProfile && !isStudent ? (departmentId ? Number(departmentId) : null) : null,
      });
      await fetchUser();
    } catch {
      setError("Ошибка при сохранении");
    } finally {
      setSaving(false);
    }
  };

  const handleResetPassword = async () => {
    if (!newPassword || newPassword.length < 6) {
      setError("Пароль должен быть не менее 6 символов");
      return;
    }
    if (!confirm(`Сбросить пароль пользователя ${userData?.fullName}?`))
      return;
    setSaving(true);
    try {
      await resetPassword(id, newPassword);
      setShowReset(false);
      setNewPassword("");
      alert("Пароль сброшен");
    } catch {
      setError("Ошибка при сбросе пароля");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async () => {
    if (!userData) return;
    if (!confirm(`Удалить пользователя ${userData.fullName}? Это действие необратимо.`))
      return;
    try {
      await deleteUser(id);
      router.push("/admin/users");
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

  if (error || !userData) {
    return (
      <div className="p-6 max-w-3xl mx-auto">
        <div className="bg-error-bg border border-error-border rounded-[16px] px-6 py-4 text-sm text-error-text">
          {error || "Пользователь не найден"}
        </div>
      </div>
    );
  }

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button
            onClick={() => router.push("/admin/users")}
            className="p-2 hover:bg-table-hover rounded-[12px] transition-colors"
          >
            <ArrowLeft className="w-5 h-5 text-text-secondary" />
          </button>
          <div>
            <h1 className="text-2xl font-bold text-text-primary">
              {userData.fullName}
            </h1>
            <p className="text-sm text-text-secondary">{userData.email}</p>
          </div>
        </div>
        <button
          onClick={handleDelete}
          className="inline-flex items-center gap-2 h-12 px-6 border border-status-rejected text-status-rejected rounded-[16px] font-semibold text-sm hover:bg-error-bg transition-colors"
        >
          <Trash2 className="w-4 h-4" />
          Удалить
        </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {/* Edit form */}
        <div
          className="bg-card-bg border border-card-border p-6"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            Редактировать
          </h2>
          <form onSubmit={handleSave} className="space-y-4">
            <Input
              label="Email"
              name="email"
              value={userData.email}
              onChange={() => {}}
              disabled
            />
            <Input
              label="Фамилия"
              name="lastName"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
            <Input
              label="Имя"
              name="firstName"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
            />
            <Input
              label="Отчество"
              name="patronymic"
              value={patronymic}
              onChange={(e) => setPatronymic(e.target.value)}
            />

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
                onChange={(e) => setRole(e.target.value)}
                className="w-full h-12 px-4 bg-input-bg border border-input-border rounded-[16px] text-text-primary focus:outline-none focus:border-[2px] focus:border-primary transition-all"
              >
                {Object.entries(ROLE_LABELS).map(([key, label]) => (
                  <option key={key} value={key}>
                    {label}
                  </option>
                ))}
              </select>
            </div>

            {refsLoading ? (
              <div className="flex justify-center py-4">
                <div className="w-5 h-5 border-2 border-primary border-t-transparent rounded-full animate-spin" />
              </div>
            ) : role === "StudentProfile" ? (
              <div className="border-t border-card-border pt-4 space-y-4">
                <p className="text-sm font-semibold text-text-primary">
                  Профиль студента
                </p>
                <Input
                  label="Номер студенческого"
                  name="studentTicket"
                  value={studentTicket}
                  onChange={(e) => setStudentTicket(e.target.value)}
                  placeholder="123456"
                />
                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">Группа</label>
                  <select
                    value={academicGroupId}
                    onChange={(e) => setAcademicGroupId(e.target.value)}
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
                  <label className="text-sm text-text-secondary">Статус</label>
                  <select
                    value={studentStatusId}
                    onChange={(e) => setStudentStatusId(e.target.value)}
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
            ) : role !== "Admin" ? (
              <div className="border-t border-card-border pt-4 space-y-4">
                <p className="text-sm font-semibold text-text-primary">
                  Профиль сотрудника
                </p>
                <div className="flex flex-col gap-1.5">
                  <label className="text-sm text-text-secondary">Кафедра</label>
                  <select
                    value={departmentId}
                    onChange={(e) => setDepartmentId(e.target.value)}
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
                {role === "Curator" && (
                  <div className="flex flex-col gap-1.5">
                    <label className="text-sm text-text-secondary">Группа (кураторство)</label>
                    <select
                      value={academicGroupId}
                      onChange={(e) => setAcademicGroupId(e.target.value)}
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
                )}
              </div>
            ) : null}

            {error && (
              <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                {error}
              </div>
            )}

            <Button type="submit" loading={saving}>
              <Save className="w-4 h-4" />
              Сохранить
            </Button>
          </form>
        </div>

        {/* Info + Reset Password */}
        <div className="space-y-6">
          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Информация
            </h2>
            <div className="space-y-3">
              <InfoRow icon={User} label="ФИО" value={userData.fullName} />
              <InfoRow icon={Mail} label="Email" value={userData.email} />
              <InfoRow
                icon={Shield}
                label="Роль"
                value={
                  ROLE_LABELS[userData.role as SystemRoleName] ||
                  userData.role
                }
              />
              {userData.studentTicket && (
                <InfoRow
                  icon={User}
                  label="Студ. билет"
                  value={userData.studentTicket}
                />
              )}
              {userData.departmentName && (
                <InfoRow
                  icon={User}
                  label="Кафедра"
                  value={userData.departmentName}
                />
              )}
              {userData.academicGroupName && (
                <InfoRow
                  icon={User}
                  label="Группа"
                  value={userData.academicGroupName}
                />
              )}
            </div>
          </div>

          <div
            className="bg-card-bg border border-card-border p-6"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <h2 className="font-semibold text-text-primary mb-4">
              Сброс пароля
            </h2>
            {!showReset ? (
              <Button
                onClick={() => setShowReset(true)}
                variant="ghost"
                className="w-full"
              >
                <Key className="w-4 h-4" />
                Сбросить пароль
              </Button>
            ) : (
              <div className="space-y-3">
                <Input
                  label="Новый пароль"
                  name="newPassword"
                  type="password"
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  placeholder="Минимум 6 символов"
                />
                <div className="flex gap-2">
                  <Button onClick={handleResetPassword} loading={saving}>
                    Подтвердить
                  </Button>
                  <Button
                    variant="ghost"
                    onClick={() => {
                      setShowReset(false);
                      setNewPassword("");
                    }}
                  >
                    Отмена
                  </Button>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

function InfoRow({
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
      <div>
        <p className="text-xs text-text-muted">{label}</p>
        <p className="text-sm font-medium text-text-primary">{value}</p>
      </div>
    </div>
  );
}
