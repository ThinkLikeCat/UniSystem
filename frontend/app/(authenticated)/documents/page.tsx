"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useAuth } from "@/components/AuthProvider";
import { getDocuments } from "@/lib/documents";
import { getDocumentTypes, getDocumentStatuses } from "@/lib/references";
import type {
  DocumentListItemDto,
  DocumentTypeDto,
  DocumentStatusDto,
} from "@/types";
import {
  Plus,
  FileText,
  Search,
  Filter,
} from "lucide-react";

export default function DocumentsPage() {
  const { user } = useAuth();
  const [documents, setDocuments] = useState<DocumentListItemDto[]>([]);
  const [docTypes, setDocTypes] = useState<DocumentTypeDto[]>([]);
  const [statuses, setStatuses] = useState<DocumentStatusDto[]>([]);
  const [loading, setLoading] = useState(true);

  const [filterType, setFilterType] = useState("");
  const [filterStatus, setFilterStatus] = useState("");
  const [searchQuery, setSearchQuery] = useState("");

  const isStudent = user?.role === "StudentProfile";

  useEffect(() => {
    const load = async () => {
      try {
        const [types, st] = await Promise.all([
          getDocumentTypes(),
          getDocumentStatuses(),
        ]);
        setDocTypes(types);
        setStatuses(st);
      } catch {
        // silent
      }
    };
    load();
  }, []);

  useEffect(() => {
    if (!user) return;
    const load = async () => {
      setLoading(true);
      try {
        const params: Record<string, string | number> = {};
        if (filterType) params.documentTypeId = Number(filterType);
        if (filterStatus) params.statusId = Number(filterStatus);
        if (isStudent) params.authorId = user.id;

        const docs = await getDocuments(params);
        setDocuments(docs);
      } catch {
        // silent
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [user, filterType, filterStatus, isStudent]);

  const filtered = documents.filter((doc) => {
    if (!searchQuery) return true;
    const q = searchQuery.toLowerCase();
    return (
      doc.documentTypeName.toLowerCase().includes(q) ||
      doc.authorName.toLowerCase().includes(q) ||
      doc.currentStatusName.toLowerCase().includes(q)
    );
  });

  if (!user) return null;

  return (
    <div className="p-6 max-w-6xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">Документы</h1>
          <p className="text-sm text-text-secondary mt-1">
            {isStudent
              ? "Ваши документы"
              : "Все документы системы"}
          </p>
        </div>
        {isStudent && (
          <Link
            href="/documents/new"
            className="inline-flex items-center gap-2 h-12 px-6 bg-primary text-white rounded-[16px] font-semibold text-base shadow-[0px_8px_20px_rgba(37,99,235,0.35)] hover:bg-primary-hover transition-colors"
          >
            <Plus className="w-5 h-5" />
            Создать документ
          </Link>
        )}
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
              placeholder="Поиск по документам..."
              className="w-full h-10 pl-9 pr-4 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary placeholder:text-text-muted focus:outline-none focus:border-[2px] focus:border-primary transition-all"
            />
          </div>

          <div className="flex items-center gap-2">
            <Filter className="w-4 h-4 text-text-muted" />
            <select
              value={filterType}
              onChange={(e) => setFilterType(e.target.value)}
              className="h-10 px-3 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary focus:outline-none focus:border-[2px] focus:border-primary"
            >
              <option value="">Все типы</option>
              {docTypes.map((t) => (
                <option key={t.id} value={t.id}>
                  {t.name}
                </option>
              ))}
            </select>

            <select
              value={filterStatus}
              onChange={(e) => setFilterStatus(e.target.value)}
              className="h-10 px-3 bg-input-bg border border-input-border rounded-[12px] text-sm text-text-primary focus:outline-none focus:border-[2px] focus:border-primary"
            >
              <option value="">Все статусы</option>
              {statuses.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.name}
                </option>
              ))}
            </select>
          </div>
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
            <FileText className="w-12 h-12 mb-3 opacity-50" />
            <p>
              {searchQuery || filterType || filterStatus
                ? "Ничего не найдено"
                : "Нет документов"}
            </p>
            {isStudent && !searchQuery && !filterType && !filterStatus && (
              <Link
                href="/documents/new"
                className="mt-2 text-sm text-primary hover:underline"
              >
                Создать первый документ
              </Link>
            )}
          </div>
        ) : (
          <>
            {/* Table header */}
            <div className="hidden md:grid grid-cols-12 gap-4 px-6 py-3 border-b border-card-border text-xs font-medium text-text-muted uppercase tracking-wider">
              <div className="col-span-1" />
              <div className="col-span-4">Тип документа</div>
              <div className="col-span-2">Автор</div>
              <div className="col-span-2">Дата</div>
              <div className="col-span-3">Статус</div>
            </div>

            <div className="divide-y divide-card-border">
              {filtered.map((doc) => (
                <Link
                  key={doc.id}
                  href={`/documents/${doc.id}`}
                  className="grid grid-cols-1 md:grid-cols-12 gap-2 md:gap-4 px-6 py-4 hover:bg-table-hover transition-colors items-center"
                >
                  <div className="hidden md:flex col-span-1">
                    <div
                      className="w-2 h-2 rounded-full"
                      style={{
                        backgroundColor: getStatusColor(doc.currentStatusName),
                      }}
                    />
                  </div>
                  <div className="md:col-span-4">
                    <p className="font-medium text-text-primary text-sm">
                      {doc.documentTypeName}
                    </p>
                    <p className="text-xs text-text-muted md:hidden mt-1">
                      {doc.authorName}
                    </p>
                  </div>
                  <div className="hidden md:block md:col-span-2 text-sm text-text-secondary">
                    {doc.authorName}
                  </div>
                  <div className="md:col-span-2 text-sm text-text-secondary">
                    {new Date(doc.createdAt).toLocaleDateString("ru-RU")}
                  </div>
                  <div className="md:col-span-3">
                    <span
                      className="inline-block text-xs font-medium px-3 py-1"
                      style={{
                        borderRadius: "var(--radius-pill)",
                        backgroundColor: `${getStatusColor(
                          doc.currentStatusName
                        )}15`,
                        color: getStatusColor(doc.currentStatusName),
                      }}
                    >
                      {doc.currentStatusName}
                    </span>
                  </div>
                </Link>
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
}

function getStatusColor(statusName: string): string {
  const map: Record<string, string> = {
    Черновик: "var(--color-status-draft)",
    "На проверке секретаря": "var(--color-status-secretary)",
    "На доработку": "var(--color-status-rework)",
    "На проверке декана": "var(--color-status-dean)",
    Утверждён: "var(--color-status-approved)",
    Отклонён: "var(--color-status-rejected)",
  };
  return map[statusName] || "var(--color-text-muted)";
}
