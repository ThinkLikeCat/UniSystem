'use client'

import { Loader2 } from "lucide-react";

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: "primary" | "ghost";
  loading?: boolean;
  children: React.ReactNode;
}

export function Button({
  variant = "primary",
  loading = false,
  children,
  className = "",
  disabled,
  ...props
}: ButtonProps) {
  const base =
    "h-12 px-6 rounded-[16px] font-semibold text-base transition-all duration-200 cursor-pointer inline-flex items-center justify-center gap-2";

  const variants: Record<string, string> = {
    primary:
      "bg-primary text-white shadow-[0px_8px_20px_rgba(37,99,235,0.35)] hover:bg-primary-hover active:bg-primary-active disabled:opacity-50 disabled:cursor-not-allowed disabled:shadow-none",
    ghost:
      "bg-transparent text-text-secondary hover:bg-table-hover active:bg-divider disabled:opacity-50 disabled:cursor-not-allowed",
  };

  return (
    <button
      className={`${base} ${variants[variant]} ${className}`}
      disabled={disabled || loading}
      {...props}
    >
      {loading && <Loader2 className="w-5 h-5 animate-spin" />}
      {children}
    </button>
  );
}
