import {
  createContext,
  useEffect,
  useState,
  type ReactNode,
} from "react";

import {
  clearStoredAuth,
  getStoredAuth,
  saveAuth,
} from "./authStorage";

import type { AuthResponse } from "./types";

interface AuthContextValue {
  auth: AuthResponse | null;
  isAuthenticated: boolean;
  login: (auth: AuthResponse) => void;
  logout: () => void;
}

export const AuthContext =
  createContext<AuthContextValue | null>(null);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({
  children,
}: AuthProviderProps) {
  const [auth, setAuth] =
    useState<AuthResponse | null>(null);

  useEffect(() => {
    const storedAuth = getStoredAuth();

    setAuth(storedAuth);
  }, []);

  function login(authResponse: AuthResponse) {
    saveAuth(authResponse);
    setAuth(authResponse);
  }

  function logout() {
    clearStoredAuth();
    setAuth(null);
  }

  const isAuthenticated = auth !== null;

  return (
    <AuthContext.Provider
      value={{
        auth,
        isAuthenticated,
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}