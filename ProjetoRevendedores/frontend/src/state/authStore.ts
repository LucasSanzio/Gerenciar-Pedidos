import { create } from 'zustand';
import { persist } from 'zustand/middleware';

export type UserRole = 'Vendedor' | 'Gestor';

export interface AuthUser {
  name: string;
  role: UserRole;
}

interface AuthState {
  user: AuthUser | null;
  login: (role: UserRole, name?: string) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      user: null,
      login: (role, name) =>
        set({
          user: {
            name: name || (role === 'Gestor' ? 'Gestor' : 'Vendedor'),
            role
          }
        }),
      logout: () => set({ user: null })
    }),
    {
      name: 'auth-storage'
    }
  )
);
