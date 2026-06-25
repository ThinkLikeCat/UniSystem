import { LoginForm } from "@/components/auth/LoginForm";

export default function LoginPage() {
  return (
    <div className="flex-1 flex items-center justify-center bg-hero-bg">
      <div
          className="w-[416px] bg-card-bg border border-card-border p-10"
          style={{
            borderRadius: "var(--radius-card)",
            boxShadow: "0px 10px 30px rgba(30, 58, 138, 0.08), inset 0px 2px 0px #FFFFFF",
          }}
        >
        <div className="flex flex-col items-center mb-8">
          <h1 className="text-[28px] font-bold text-primary-dark">UniSystem</h1>
          <p className="text-sm text-text-muted mt-1">
            Университетская система управления документами
          </p>
        </div>

        <LoginForm />
      </div>
    </div>
  );
}
