"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/Button";
import { Input } from "@/components/ui/Input";
import { changePassword } from "@/lib/profile";
import { useAuth } from "@/components/AuthProvider";
import { ArrowLeft, LogOut } from "lucide-react";

export default function ChangePasswordPage() {
  const router = useRouter();
  const { logout } = useAuth();
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    if (!currentPassword) {
      setError("Введите текущий пароль");
      return;
    }
    if (!newPassword) {
      setError("Введите новый пароль");
      return;
    }
    if (newPassword.length < 6) {
      setError("Новый пароль должен быть не менее 6 символов");
      return;
    }
    if (newPassword !== confirmPassword) {
      setError("Пароли не совпадают");
      return;
    }

    setSaving(true);

    try {
      await changePassword({ currentPassword, newPassword });
      setSuccess(true);
      setTimeout(() => {
        logout();
      }, 2000);
    } catch (err: unknown) {
      const data =
        err && typeof err === "object" && "response" in err
          ? (err as any).response?.data
          : null;
      const msg = data?.details
        ? (data.details as string[]).join("; ")
        : data?.error;
      setError(msg || "Неверный текущий пароль");
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
          Сменить пароль
        </h1>
      </div>

      <div
        className="bg-card-bg border border-card-border p-6"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        {success ? (
          <div className="text-center py-8 space-y-3">
            <div className="text-green-600 font-semibold text-lg">
              Пароль успешно изменён
            </div>
            <p className="text-sm text-text-secondary">
              Вы будете перенаправлены на страницу входа
            </p>
            <div className="flex justify-center">
              <div className="w-6 h-6 border-2 border-primary border-t-transparent rounded-full animate-spin" />
            </div>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-5">
            <Input
              label="Текущий пароль"
              name="currentPassword"
              type="password"
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              placeholder="Введите текущий пароль"
            />

            <div>
              <Input
                label="Новый пароль"
                name="newPassword"
                type="password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                placeholder="Введите новый пароль (мин. 6 символов)"
              />
              <p className="mt-1 text-xs text-text-muted">
                Пароль должен содержать минимум 6 символов, заглавную и строчную
                буквы, цифру и спецсимвол
              </p>
            </div>

            <Input
              label="Подтверждение пароля"
              name="confirmPassword"
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              placeholder="Повторите новый пароль"
            />

            {error && (
              <div className="bg-error-bg border border-error-border rounded-[12px] px-4 py-3 text-sm text-error-text">
                {error}
              </div>
            )}

            <div className="flex gap-3">
              <Button type="submit" loading={saving}>
                Сменить пароль
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
