"use client";

import { useEffect, useState } from "react";
import {
  getStatisticsSummary,
  getStatisticsByMonth,
  getStatisticsByType,
  getStatisticsResolution,
  getStatisticsByStatus,
} from "@/lib/admin";
import type {
  StatisticsSummaryDto,
  MonthlyStatDto,
  TypeStatDto,
  ResolutionStatDto,
  StatusStatDto,
} from "@/types";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  PieChart,
  Pie,
  Cell,
  Legend,
} from "recharts";
import {
  BarChart3,
  FileText,
  Clock,
  CheckCircle2,
  XCircle,
  AlertTriangle,
} from "lucide-react";

const MONTH_NAMES = [
  "Янв", "Фев", "Мар", "Апр", "Май", "Июн",
  "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек",
];

const STATUS_PIE_COLORS = [
  "#94A3B8",
  "#D97706",
  "#F97316",
  "#0E7490",
  "#059669",
  "#DC2626",
];

export default function StatisticsPage() {
  const [summary, setSummary] = useState<StatisticsSummaryDto | null>(null);
  const [byMonth, setByMonth] = useState<MonthlyStatDto[]>([]);
  const [byType, setByType] = useState<TypeStatDto[]>([]);
  const [resolution, setResolution] = useState<ResolutionStatDto | null>(null);
  const [byStatus, setByStatus] = useState<StatusStatDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAll = async () => {
      setLoading(true);
      try {
        const [s, m, t, r, st] = await Promise.all([
          getStatisticsSummary(),
          getStatisticsByMonth(),
          getStatisticsByType(),
          getStatisticsResolution(),
          getStatisticsByStatus(),
        ]);
        setSummary(s);
        setByMonth(m);
        setByType(t);
        setResolution(r);
        setByStatus(st);
      } catch {
        // silent
      } finally {
        setLoading(false);
      }
    };
    fetchAll();
  }, []);

  if (loading) {
    return (
      <div className="flex-1 flex items-center justify-center">
        <div className="w-8 h-8 border-2 border-primary border-t-transparent rounded-full animate-spin" />
      </div>
    );
  }

  const statusCards = summary
    ? [
        {
          label: "Всего документов",
          value: summary.totalDocuments,
          icon: FileText,
          color: "text-primary",
          bg: "bg-primary/5",
        },
        {
          label: "Черновики",
          value: summary.draft,
          icon: Clock,
          color: "text-status-draft",
          bg: "bg-status-draft/5",
        },
        {
          label: "На проверке секретаря",
          value: summary.underSecretaryReview,
          icon: AlertTriangle,
          color: "text-status-secretary",
          bg: "bg-status-secretary/5",
        },
        {
          label: "На доработку",
          value: summary.rework,
          icon: AlertTriangle,
          color: "text-status-rework",
          bg: "bg-status-rework/5",
        },
        {
          label: "На проверке декана",
          value: summary.underDeanReview,
          icon: Clock,
          color: "text-status-dean",
          bg: "bg-status-dean/5",
        },
        {
          label: "Утверждено",
          value: summary.approved,
          icon: CheckCircle2,
          color: "text-status-approved",
          bg: "bg-status-approved/5",
        },
        {
          label: "Отклонено",
          value: summary.rejected,
          icon: XCircle,
          color: "text-status-rejected",
          bg: "bg-status-rejected/5",
        },
      ]
    : [];

  const monthChartData = byMonth.map((m) => ({
    name: `${MONTH_NAMES[m.month - 1]} ${m.year}`,
    count: m.count,
  }));

  const typeChartData = byType.map((t) => ({
    name: t.documentTypeName,
    count: t.count,
  }));

  const pieData = byStatus.map((s, i) => ({
    name: s.statusName,
    value: s.count,
    color: STATUS_PIE_COLORS[i % STATUS_PIE_COLORS.length],
  }));

  return (
    <div className="p-6 max-w-7xl mx-auto space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-text-primary">Статистика</h1>
        <p className="text-sm text-text-secondary mt-1">
          Общая статистика по документам
        </p>
      </div>

      {summary && (
        <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-7 gap-4">
          {statusCards.map((card) => (
            <div
              key={card.label}
              className="bg-card-bg border border-card-border p-4"
              style={{
                borderRadius: "var(--radius-card)",
                boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
              }}
            >
              <div
                className={`w-10 h-10 rounded-[12px] ${card.bg} flex items-center justify-center mb-3`}
              >
                <card.icon className={`w-5 h-5 ${card.color}`} />
              </div>
              <p className="text-2xl font-bold text-text-primary">
                {card.value}
              </p>
              <p className="text-xs text-text-muted mt-1">{card.label}</p>
            </div>
          ))}
        </div>
      )}

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div
          className="bg-card-bg border border-card-border p-6"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            Динамика по месяцам
          </h2>
          {monthChartData.length === 0 ? (
            <p className="text-sm text-text-muted text-center py-8">
              Нет данных
            </p>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={monthChartData}>
                <CartesianGrid strokeDasharray="3 3" stroke="#E2E8F0" />
                <XAxis
                  dataKey="name"
                  tick={{ fontSize: 12, fill: "#94A3B8" }}
                />
                <YAxis tick={{ fontSize: 12, fill: "#94A3B8" }} />
                <Tooltip />
                <Bar dataKey="count" fill="#2563EB" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>

        <div
          className="bg-card-bg border border-card-border p-6"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            По типам документов
          </h2>
          {typeChartData.length === 0 ? (
            <p className="text-sm text-text-muted text-center py-8">
              Нет данных
            </p>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <BarChart data={typeChartData} layout="vertical">
                <CartesianGrid strokeDasharray="3 3" stroke="#E2E8F0" />
                <XAxis type="number" tick={{ fontSize: 12, fill: "#94A3B8" }} />
                <YAxis
                  dataKey="name"
                  type="category"
                  tick={{ fontSize: 12, fill: "#94A3B8" }}
                  width={120}
                />
                <Tooltip />
                <Bar dataKey="count" fill="#0E7490" radius={[0, 6, 6, 0]} />
              </BarChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div
          className="bg-card-bg border border-card-border p-6"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            Резолюции
          </h2>
          {resolution ? (
            <div className="space-y-4">
              <div className="flex items-center justify-between p-4 bg-status-approved/5 rounded-[12px]">
                <div className="flex items-center gap-3">
                  <CheckCircle2 className="w-5 h-5 text-status-approved" />
                  <span className="text-sm text-text-primary">
                    С резолюцией
                  </span>
                </div>
                <span className="text-lg font-bold text-text-primary">
                  {resolution.withResolution}
                </span>
              </div>
              <div className="flex items-center justify-between p-4 bg-status-rejected/5 rounded-[12px]">
                <div className="flex items-center gap-3">
                  <XCircle className="w-5 h-5 text-status-rejected" />
                  <span className="text-sm text-text-primary">
                    Без резолюции
                  </span>
                </div>
                <span className="text-lg font-bold text-text-primary">
                  {resolution.withoutResolution}
                </span>
              </div>
              <div className="border-t border-card-border pt-4 flex items-center justify-between">
                <span className="text-sm text-text-secondary">Всего</span>
                <span className="text-lg font-bold text-text-primary">
                  {resolution.total}
                </span>
              </div>
            </div>
          ) : (
            <p className="text-sm text-text-muted text-center py-8">
              Нет данных
            </p>
          )}
        </div>

        <div
          className="bg-card-bg border border-card-border p-6"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "var(--shadow-card), var(--shadow-card-inset)",
          }}
        >
          <h2 className="font-semibold text-text-primary mb-4">
            По статусам
          </h2>
          {pieData.length === 0 ? (
            <p className="text-sm text-text-muted text-center py-8">
              Нет данных
            </p>
          ) : (
            <ResponsiveContainer width="100%" height={300}>
              <PieChart>
                <Pie
                  data={pieData}
                  cx="50%"
                  cy="50%"
                  innerRadius={60}
                  outerRadius={100}
                  dataKey="value"
                  label={({ name, value }) => `${name}: ${value}`}
                  labelLine={false}
                >
                  {pieData.map((entry, index) => (
                    <Cell key={index} fill={entry.color} />
                  ))}
                </Pie>
                <Tooltip />
                <Legend />
              </PieChart>
            </ResponsiveContainer>
          )}
        </div>
      </div>
    </div>
  );
}
