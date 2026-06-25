"use client";

import Link from "next/link";
import { useAuth } from "@/components/AuthProvider";
import { ROLE_LABELS, type SystemRoleName } from "@/types";
import {
  User,
  Mail,
  GraduationCap,
  Building2,
  BookOpen,
  Ticket,
  Shield,
  Edit3,
  Key,
  Calendar,
} from "lucide-react";

export default function ProfilePage() {
  const { user } = useAuth();

  if (!user) return null;

  const profile = user.studentProfile || user.staffProfile;

  return (
    <div className="p-6 max-w-3xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-text-primary">Профиль</h1>
        <div className="flex gap-3">
          <Link
            href="/profile/edit"
            className="inline-flex items-center gap-2 h-12 px-6 bg-primary text-white rounded-[16px] font-semibold text-sm shadow-[0px_8px_20px_rgba(37,99,235,0.35)] hover:bg-primary-hover transition-colors"
          >
            <Edit3 className="w-4 h-4" />
            Редактировать
          </Link>
          <Link
            href="/profile/change-password"
            className="inline-flex items-center gap-2 h-12 px-6 bg-card-bg border border-card-border text-text-primary rounded-[16px] font-semibold text-sm hover:bg-table-hover transition-colors"
          >
            <Key className="w-4 h-4" />
            Сменить пароль
          </Link>
        </div>
      </div>

      <div
        className="bg-card-bg border border-card-border"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        <div className="px-6 py-5 flex items-center gap-4 border-b border-card-border">
          <div className="w-16 h-16 rounded-full bg-primary flex items-center justify-center text-white text-xl font-bold">
            {user.firstName[0]}
            {user.lastName[0]}
          </div>
          <div>
            <h2 className="text-lg font-semibold text-text-primary">
              {user.lastName} {user.firstName}
              {user.patronymic && ` ${user.patronymic}`}
            </h2>
            <p className="text-sm text-text-secondary">{user.email}</p>
          </div>
        </div>

        <div className="px-6 py-5 space-y-5">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
            <Field icon={User} label="Имя" value={user.firstName} />
            <Field icon={User} label="Фамилия" value={user.lastName} />
            {user.patronymic && (
              <Field icon={User} label="Отчество" value={user.patronymic} />
            )}
            <Field icon={Mail} label="Email" value={user.email} />
            <Field
              icon={Shield}
              label="Роль"
              value={ROLE_LABELS[user.role as SystemRoleName] || user.role}
            />
            <Field
              icon={Calendar}
              label="Пол"
              value={
                user.sex === "Male"
                  ? "Мужской"
                  : user.sex === "Female"
                  ? "Женский"
                  : "Другой"
              }
            />
          </div>

          {profile && (
            <>
              <div className="border-t border-card-border pt-5">
                <h3 className="font-semibold text-text-primary mb-4">
                  {user.studentProfile
                    ? "Информация студента"
                    : "Информация сотрудника"}
                </h3>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                  {user.studentProfile && (
                    <>
                      <Field
                        icon={Ticket}
                        label="Номер студенческого"
                        value={user.studentProfile.studentTicket}
                      />
                      <Field
                        icon={GraduationCap}
                        label="Группа"
                        value={
                          user.studentProfile.academicGroupName || "—"
                        }
                      />
                      <Field
                        icon={BookOpen}
                        label="Статус"
                        value={
                          user.studentProfile.studentStatusName || "—"
                        }
                      />
                    </>
                  )}
                  {user.staffProfile && (
                    <>
                      <Field
                        icon={Building2}
                        label="Кафедра"
                        value={
                          user.staffProfile.departmentName || "—"
                        }
                      />
                      <Field
                        icon={GraduationCap}
                        label="Группа"
                        value={
                          user.staffProfile.academicGroupName || "—"
                        }
                      />
                    </>
                  )}
                </div>
              </div>
            </>
          )}
        </div>
      </div>
    </div>
  );
}

function Field({
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
      <div className="min-w-0">
        <p className="text-xs text-text-muted">{label}</p>
        <p className="text-sm font-medium text-text-primary truncate">
          {value}
        </p>
      </div>
    </div>
  );
}
