"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { useAuth } from "@/components/AuthProvider";
import { getDocuments } from "@/lib/documents";
import { getStatisticsSummary } from "@/lib/admin";
import type {
  DocumentListItemDto,
  StatisticsSummaryDto,
} from "@/types";
import {
  FileText,
  Plus,
  FileCheck,
  Clock,
  RefreshCw,
  XCircle,
  CheckCircle,
  Send,
} from "lucide-react";

export default function Dashboard() {
  const { user } = useAuth();
  const [documents, setDocuments] = useState<DocumentListItemDto[]>([]);
  const [stats, setStats] = useState<StatisticsSummaryDto | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!user) return;

    const load = async () => {
      try {
        const [docs, summary] = await Promise.all([
          getDocuments(
            user.role === "StudentProfile"
              ? { authorId: user.id }
              : undefined
          ),
          user.role === "Admin" ? getStatisticsSummary() : Promise.resolve(null),
        ]);
        setDocuments(docs);
        setStats(summary);
      } catch {
        // silently fail
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [user]);

  if (!user) return null;

  const isStudent = user.role === "StudentProfile";
  const isSecretary = user.role === "Secretary";
  const isDean = user.role === "Dean";
  const isAdmin = user.role === "Admin";

  const pendingReview = documents.filter(
    (d) =>
      d.currentStatusName === "На проверке секретаря" ||
      d.currentStatusName === "На проверке декана"
  );

  const statusCounts = documents.reduce<Record<string, number>>(
    (acc, d) => {
      acc[d.currentStatusName] = (acc[d.currentStatusName] || 0) + 1;
      return acc;
    },
    {}
  );

  const statusCards = [
    {
      label: "Черновик",
      count: statusCounts["Черновик"] || 0,
      color: "var(--color-status-draft)",
      icon: FileText,
    },
    {
      label: "На проверке",
      count:
        (statusCounts["На проверке секретаря"] || 0) +
        (statusCounts["На проверке декана"] || 0),
      color: "var(--color-status-secretary)",
      icon: Clock,
    },
    {
      label: "На доработку",
      count: statusCounts["На доработку"] || 0,
      color: "var(--color-status-rework)",
      icon: RefreshCw,
    },
    {
      label: "Утверждён",
      count: statusCounts["Утверждён"] || 0,
      color: "var(--color-status-approved)",
      icon: CheckCircle,
    },
    {
      label: "Отклонён",
      count: statusCounts["Отклонён"] || 0,
      color: "var(--color-status-rejected)",
      icon: XCircle,
    },
  ];

  return (
    <div className="p-6 max-w-6xl mx-auto space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-text-primary">
            {isAdmin
              ? "Панель управления"
              : isStudent
              ? "Мои документы"
              : "Документы"}
          </h1>
          <p className="text-text-secondary mt-1">
            {isAdmin
              ? "Общая статистика системы"
              : isStudent
              ? "Управляйте своими документами"
              : isSecretary || isDean
              ? `Ожидают проверки: ${pendingReview.length}`
              : `Всего документов: ${documents.length}`}
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

      {loading ? (
        <div className="flex justify-center py-12">
          <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
        </div>
      ) : (
        <>
          {isAdmin && stats && (
            <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
              <StatCard
                label="Всего"
                value={stats.totalDocuments}
                color="var(--color-primary)"
              />
              <StatCard
                label="Черновики"
                value={stats.draft}
                color="var(--color-status-draft)"
              />
              <StatCard
                label="На проверке секретаря"
                value={stats.underSecretaryReview}
                color="var(--color-status-secretary)"
              />
              <StatCard
                label="На доработку"
                value={stats.rework}
                color="var(--color-status-rework)"
              />
              <StatCard
                label="На проверке декана"
                value={stats.underDeanReview}
                color="var(--color-status-dean)"
              />
              <StatCard
                label="Утверждены"
                value={stats.approved}
                color="var(--color-status-approved)"
              />
              <StatCard
                label="Отклонены"
                value={stats.rejected}
                color="var(--color-status-rejected)"
              />
            </div>
          )}

          {!isAdmin && (
            <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
              {statusCards.map((card) => (
                <div
                  key={card.label}
                  className="bg-card-bg border border-card-border p-4"
                  style={{
                    borderRadius: "var(--radius-card)",
                    boxShadow:
                      "var(--shadow-card), var(--shadow-card-inset)",
                  }}
                >
                  <div className="flex items-center gap-2 mb-2">
                    <card.icon
                      className="w-4 h-4"
                      style={{ color: card.color }}
                    />
                    <span className="text-xs text-text-secondary">
                      {card.label}
                    </span>
                  </div>
                  <p
                    className="text-2xl font-bold"
                    style={{ color: card.color }}
                  >
                    {card.count}
                  </p>
                </div>
              ))}
            </div>
          )}

          <div className="bg-card-bg border border-card-border"
            style={{
              borderRadius: "var(--radius-card)",
              boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
            }}
          >
            <div className="flex items-center justify-between px-6 py-4 border-b border-card-border">
              <h2 className="font-semibold text-text-primary">
                {isStudent ? "Последние документы" : "Все документы"}
              </h2>
              <Link
                href="/documents"
                className="text-sm text-primary hover:underline"
              >
                Все документы →
              </Link>
            </div>

            {documents.length === 0 ? (
              <div className="flex flex-col items-center py-12 text-text-muted">
                <FileText className="w-12 h-12 mb-3 opacity-50" />
                <p>Нет документов</p>
                {isStudent && (
                  <Link
                    href="/documents/new"
                    className="mt-2 text-sm text-primary hover:underline"
                  >
                    Создать первый документ
                  </Link>
                )}
              </div>
            ) : (
              <div className="divide-y divide-card-border">
                {documents.slice(0, 10).map((doc) => (
                  <Link
                    key={doc.id}
                    href={`/documents/${doc.id}`}
                    className="flex items-center justify-between px-6 py-4 hover:bg-table-hover transition-colors"
                  >
                    <div className="flex items-center gap-4 min-w-0">
                      <div
                        className="w-2 h-2 rounded-full shrink-0"
                        style={{
                          backgroundColor: getStatusColor(
                            doc.currentStatusName
                          ),
                        }}
                      />
                      <div className="min-w-0">
                        <p className="font-medium text-text-primary truncate">
                          {doc.documentTypeName}
                        </p>
                        <p className="text-sm text-text-secondary">
                          {doc.authorName} ·{" "}
                          {new Date(doc.createdAt).toLocaleDateString("ru-RU")}
                        </p>
                      </div>
                    </div>
                    <span
                      className="text-xs font-medium px-3 py-1 shrink-0"
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
                  </Link>
                ))}
              </div>
            )}
          </div>

          {(isSecretary || isDean) && pendingReview.length > 0 && (
            <div className="bg-card-bg border border-card-border p-6"
              style={{
                borderRadius: "var(--radius-card)",
                boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
              }}
            >
              <div className="flex items-center gap-3 mb-4">
                <Send className="w-5 h-5 text-primary" />
                <h2 className="font-semibold text-text-primary">
                  Документы на проверке
                </h2>
              </div>
              <div className="space-y-2">
                {pendingReview.slice(0, 5).map((doc) => (
                  <Link
                    key={doc.id}
                    href={`/documents/${doc.id}/review`}
                    className="flex items-center justify-between px-4 py-3 rounded-[12px] hover:bg-table-hover transition-colors"
                  >
                    <span className="text-sm font-medium text-text-primary">
                      {doc.documentTypeName}
                    </span>
                    <span className="text-xs text-text-secondary">
                      {doc.authorName} ·{" "}
                      {new Date(doc.createdAt).toLocaleDateString("ru-RU")}
                    </span>
                  </Link>
                ))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}

function StatCard({
  label,
  value,
  color,
}: {
  label: string;
  value: number;
  color: string;
}) {
  return (
    <div
      className="bg-card-bg border border-card-border p-4"
      style={{
        borderRadius: "var(--radius-card)",
        boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
      }}
    >
      <p className="text-xs text-text-secondary mb-1">{label}</p>
      <p className="text-2xl font-bold" style={{ color }}>
        {value}
      </p>
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
