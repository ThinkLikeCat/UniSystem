"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { useAuth } from "@/components/AuthProvider";
import {
  LayoutDashboard,
  FileText,
  User,
  Users,
  BarChart3,
  BookOpen,
  BookMarked,
  BookCheck,
  GraduationCap,
  FolderOpen,
  Library,
  BookType,
  ClipboardList,
  LogOut,
  ChevronDown,
  ChevronRight,
} from "lucide-react";
import { useState } from "react";

const adminRefLinks = [
  { href: "/admin/references/departments", label: "Кафедры", icon: BookOpen },
  {
    href: "/admin/references/specialties",
    label: "Специальности",
    icon: BookMarked,
  },
  {
    href: "/admin/references/academic-groups",
    label: "Группы",
    icon: GraduationCap,
  },
  { href: "/admin/references/subjects", label: "Предметы", icon: Library },
  {
    href: "/admin/references/document-types",
    label: "Типы документов",
    icon: BookType,
  },
  {
    href: "/admin/references/document-statuses",
    label: "Статусы документов",
    icon: BookCheck,
  },
  {
    href: "/admin/references/student-statuses",
    label: "Статусы студентов",
    icon: FolderOpen,
  },
];

export function Sidebar() {
  const pathname = usePathname();
  const { user, logout } = useAuth();
  const [refOpen, setRefOpen] = useState(false);
  const [docOpen, setDocOpen] = useState(true);

  if (!user) return null;

  const isAdmin = user.role === "Admin";
  const isStudent = user.role === "StudentProfile";

  const linkClass = (href: string) =>
    `flex items-center gap-3 px-4 py-2.5 rounded-[16px] text-sm transition-colors ${
      pathname === href || pathname.startsWith(href + "/")
        ? "bg-primary/10 text-primary font-semibold"
        : "text-header-text-muted hover:text-header-text hover:bg-white/5"
    }`;

  return (
    <aside className="w-64 bg-header-bg min-h-screen flex flex-col shrink-0">
      <div className="px-6 py-5 border-b border-white/10">
        <Link href="/" className="text-xl font-bold text-header-text">
          UniSystem
        </Link>
      </div>

      <nav className="flex-1 px-3 py-4 space-y-1 overflow-y-auto">
        <Link href="/" className={linkClass("/")}>
          <LayoutDashboard className="w-5 h-5 shrink-0" />
          Главная
        </Link>

        <div>
          <button
            onClick={() => setDocOpen(!docOpen)}
            className={`flex items-center justify-between w-full px-4 py-2.5 rounded-[16px] text-sm transition-colors ${
              pathname.startsWith("/documents")
                ? "bg-primary/10 text-primary font-semibold"
                : "text-header-text-muted hover:text-header-text hover:bg-white/5"
            }`}
          >
            <span className="flex items-center gap-3">
              <FileText className="w-5 h-5 shrink-0" />
              Документы
            </span>
            {docOpen ? (
              <ChevronDown className="w-4 h-4" />
            ) : (
              <ChevronRight className="w-4 h-4" />
            )}
          </button>
          {docOpen && (
            <div className="ml-4 mt-1 space-y-1">
              <Link
                href="/documents"
                className={`block px-4 py-2 rounded-[16px] text-sm transition-colors ${
                  pathname === "/documents"
                    ? "bg-primary/10 text-primary font-semibold"
                    : "text-header-text-muted hover:text-header-text hover:bg-white/5"
                }`}
              >
                Список
              </Link>
              {isStudent && (
                <Link
                  href="/documents/new"
                  className={`block px-4 py-2 rounded-[16px] text-sm transition-colors ${
                    pathname === "/documents/new"
                      ? "bg-primary/10 text-primary font-semibold"
                      : "text-header-text-muted hover:text-header-text hover:bg-white/5"
                  }`}
                >
                  Создать
                </Link>
              )}
            </div>
          )}
        </div>

        <Link href="/profile" className={linkClass("/profile")}>
          <User className="w-5 h-5 shrink-0" />
          Профиль
        </Link>

        {isAdmin && (
          <>
            <div className="border-t border-white/10 my-3" />

            <Link href="/admin/users" className={linkClass("/admin/users")}>
              <Users className="w-5 h-5 shrink-0" />
              Пользователи
            </Link>

            <Link
              href="/admin/statistics"
              className={linkClass("/admin/statistics")}
            >
              <BarChart3 className="w-5 h-5 shrink-0" />
              Статистика
            </Link>

            <div>
              <button
                onClick={() => setRefOpen(!refOpen)}
                className={`flex items-center justify-between w-full px-4 py-2.5 rounded-[16px] text-sm transition-colors ${
                  pathname.startsWith("/admin/references")
                    ? "bg-primary/10 text-primary font-semibold"
                    : "text-header-text-muted hover:text-header-text hover:bg-white/5"
                }`}
              >
                <span className="flex items-center gap-3">
                  <ClipboardList className="w-5 h-5 shrink-0" />
                  Справочники
                </span>
                {refOpen ? (
                  <ChevronDown className="w-4 h-4" />
                ) : (
                  <ChevronRight className="w-4 h-4" />
                )}
              </button>
              {refOpen && (
                <div className="ml-4 mt-1 space-y-1">
                  {adminRefLinks.map((link) => (
                    <Link
                      key={link.href}
                      href={link.href}
                      className={`flex items-center gap-3 px-4 py-2 rounded-[16px] text-sm transition-colors ${
                        pathname === link.href
                          ? "bg-primary/10 text-primary font-semibold"
                          : "text-header-text-muted hover:text-header-text hover:bg-white/5"
                      }`}
                    >
                      <link.icon className="w-4 h-4 shrink-0" />
                      {link.label}
                    </Link>
                  ))}
                </div>
              )}
            </div>

            <Link
              href="/admin/staff-subjects"
              className={linkClass("/admin/staff-subjects")}
            >
              <BookOpen className="w-5 h-5 shrink-0" />
              Предметы сотрудников
            </Link>
          </>
        )}
      </nav>

      <div className="px-4 py-4 border-t border-white/10">
        <div className="flex items-center gap-3 px-2 mb-3">
          <div className="w-8 h-8 rounded-full bg-primary flex items-center justify-center text-white text-sm font-semibold">
            {user.firstName[0]}
            {user.lastName[0]}
          </div>
          <div className="flex-1 min-w-0">
            <p className="text-sm font-medium text-header-text truncate">
              {user.firstName} {user.lastName}
            </p>
            <p className="text-xs text-header-text-muted truncate">
              {user.email}
            </p>
          </div>
        </div>
        <button
          onClick={logout}
          className="flex items-center gap-3 w-full px-4 py-2.5 rounded-[16px] text-sm text-header-text-muted hover:text-header-text hover:bg-white/5 transition-colors"
        >
          <LogOut className="w-5 h-5 shrink-0" />
          Выйти
        </button>
      </div>
    </aside>
  );
}
