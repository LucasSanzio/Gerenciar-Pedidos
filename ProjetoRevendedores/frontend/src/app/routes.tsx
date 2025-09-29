import { Navigate, Outlet, Route, Routes } from 'react-router-dom';
import { AppLayout, PublicLayout } from './layout';
import CatalogoPage from '../pages/Catalogo';
import CarrinhoPage from '../pages/Carrinho';
import AdminPage from '../pages/Admin';
import DashboardPage from '../pages/Dashboard';
import LoginPage from '../pages/Login';
import { useAuthStore, type UserRole } from '../state/authStore';
import { PropsWithChildren } from 'react';

interface RequireAuthProps {
  roles?: UserRole[];
}

function RequireAuth({ roles, children }: PropsWithChildren<RequireAuthProps>) {
  const user = useAuthStore((state) => state.user);

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  if (roles && !roles.includes(user.role)) {
    return <Navigate to="/catalogo" replace />;
  }

  return <>{children}</>;
}

function ProtectedRoute({ roles }: RequireAuthProps) {
  return (
    <RequireAuth roles={roles}>
      <Outlet />
    </RequireAuth>
  );
}

export default function AppRoutes() {
  return (
    <Routes>
      <Route element={<PublicLayout />}>
        <Route path="/login" element={<LoginPage />} />
      </Route>

      <Route element={<AppLayout />}>
        <Route index element={<Navigate to="/catalogo" replace />} />
        <Route element={<ProtectedRoute roles={['Vendedor', 'Gestor']} />}>
          <Route path="/catalogo" element={<CatalogoPage />} />
          <Route path="/carrinho" element={<CarrinhoPage />} />
          <Route path="/dashboard" element={<DashboardPage />} />
        </Route>
        <Route element={<ProtectedRoute roles={['Gestor']} />}>
          <Route path="/admin" element={<AdminPage />} />
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/catalogo" replace />} />
    </Routes>
  );
}
