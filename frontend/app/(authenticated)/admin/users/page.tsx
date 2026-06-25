"use client";

import { useEffect, useState } from "react";
import { getUsers, deleteUser } from "@/lib/admin";
import { Button } from "@/components/ui/Button";
import type { UserListItemDto, SystemRoleName } from "@/types";
import { ROLE_LABELS } from "@/types";
import { Plus, Users as UsersIcon, Search, Trash2 } from "lucide-react";
import Link from "next/link";

const roles: SystemRoleName[] = [
  "StudentProfile",
  "StaffProfile",
  "Dean",
  "Secretary",
  "Curator",
  "Admin",
];

export default function AdminUsersPage() {
  const [users, setUsers] = useState<UserListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [roleFilter, setRoleFilter] = useState("");
  const [searchQuery, setSearchQuery] = useState("");

  const fetchUsers = async () => {
    setLoading(true);
    try {
      const data = await getUsers(roleFilter || undefined);
      setUsers(data);
    } catch {
      // silent
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, [roleFilter]);

  const handleDelete = async (id: string, name: string) => {
    if (!confirm(`Удалить пользователя ${name}?`)) return;
    try {
      await deleteUser(id);
      await fetchUsers();
    } catch {
      alert("Ошибка при удалении");
    }
  };

  const filtered = users.filter((u) => {
    if (!searchQuery) return true;
    const q = searchQuery.toLowerCase();
    return (
      u.fullName.toLowerCase().includes(q) ||
      u.email.toLowerCase().includes(q)
    );
  });

  return (
    <div className="p-6 max-w-6xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">
            Пользователи
          </h1>
          <p className="text-sm text-text-secondary mt-1">
            Всего: {users.length}
          </p>
        </div>
        <Link
          href="/admin/users/new"
          className="inline-flex items-center gap-2 h-12 px-6 bg-primary text-white rounded-[16px] font-semibold text-sm shadow-[0px_8px_20px_rgba(37,99,235,0.35)] hover:bg-primary-hover transition-colors"
        >
          <Plus className="w-5 h-5" />
          Создать пользователя
        </Link>
      </div>

      <div
        className="bg-card-bg border border-card-border p-4"
        style={{
          borderRadius: "var(--radius-card)",
          boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
        }}
      >
        <div className="flex flex-wrap gap-3 items-center">
          <div className="relative flex-1 min-w-[200px]">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-text-muted" />
            <input
              type="text"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              placeholder="Поиск по имени или email..."
              className="w-full h-10 pl-9 pr-4 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all"
            />
          </div>

          <select
            value={roleFilter}
            onChange={(e) => setRoleFilter(e.target.value)}
            className="h-10 px-3 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary focus:outline-none focus:border-[2px] focus:border-primary"
          >
            <option value="">Все роли</option>
            {roles.map((r) => (
              <option key={r} value={r}>
                {ROLE_LABELS[r]}
              </option>
            ))}
          </select>
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
            <UsersIcon className="w-12 h-12 mb-3 opacity-50" />
            <p>Пользователи не найдены</p>
          </div>
        ) : (
          <>
            <div className="hidden md:grid grid-cols-12 gap-4 px-6 py-3 border-b border-card-border text-xs font-medium text-text-muted uppercase tracking-wider">
              <div className="col-span-4">ФИО</div>
              <div className="col-span-3">Email</div>
              <div className="col-span-2">Роль</div>
              <div className="col-span-3">Действия</div>
            </div>

            <div className="divide-y divide-card-border">
              {filtered.map((u) => (
                <div
                  key={u.id}
                  className="grid grid-cols-1 md:grid-cols-12 gap-2 md:gap-4 px-6 py-4 items-center"
                >
                  <div className="md:col-span-4">
                    <Link
                      href={`/admin/users/${u.id}`}
                      className="font-medium text-text-primary text-sm hover:underline"
                    >
                      {u.fullName}
                    </Link>
                  </div>
                  <div className="md:col-span-3 text-sm text-text-secondary">
                    {u.email}
                  </div>
                  <div className="md:col-span-2">
                    <span className="text-xs font-medium px-3 py-1 rounded-full bg-primary/10 text-primary">
                      {ROLE_LABELS[u.role as SystemRoleName] || u.role}
                    </span>
                  </div>
                  <div className="md:col-span-3 flex items-center gap-2">
                    <Link
                      href={`/admin/users/${u.id}`}
                      className="px-3 py-1.5 text-xs font-medium text-primary hover:bg-primary/5 rounded-[8px] transition-colors"
                    >
                      Редактировать
                    </Link>
                    <button
                      onClick={() => handleDelete(u.id, u.fullName)}
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
    </div>
  );
}
