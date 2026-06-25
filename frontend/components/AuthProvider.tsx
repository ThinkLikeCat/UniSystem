"use client";

import {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
  type ReactNode,
} from "react";
import { useRouter } from "next/navigation";
import { login as loginApi, getMe } from "@/lib/auth";
import type {
  CurrentUserResponse,
  LoginRequest,
  LoginResponse,
} from "@/types";

interface AuthContextValue {
  user: CurrentUserResponse | null;
  token: string | null;
  loading: boolean;
  error: string;
  login: (data: LoginRequest) => Promise<LoginResponse>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<CurrentUserResponse | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const router = useRouter();

  const fetchUser = useCallback(async (jwt: string) => {
    try {
      const currentUser = await getMe();
      setUser(currentUser);
      setToken(jwt);
    } catch {
      localStorage.removeItem("token");
      setUser(null);
      setToken(null);
    }
  }, []);

  useEffect(() => {
    const stored = localStorage.getItem("token");
    if (stored) {
      fetchUser(stored).finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, [fetchUser]);

  const login = useCallback(
    async (data: LoginRequest) => {
      setError("");
      const response = await loginApi(data);
      localStorage.setItem("token", response.token);
      await fetchUser(response.token);
      router.push("/");
      return response;
    },
    [fetchUser, router]
  );

  const logout = useCallback(() => {
    localStorage.removeItem("token");
    setUser(null);
    setToken(null);
    router.push("/login");
  }, [router]);

  return (
    <AuthContext.Provider value={{ user, token, loading, error, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
