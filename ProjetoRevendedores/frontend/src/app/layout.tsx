import { Fragment } from 'react';
import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../state/authStore';
import { SyncStatusBanner } from '../components/SyncStatusBanner';

export function AppLayout() {
  const user = useAuthStore((state) => state.user);
  const logout = useAuthStore((state) => state.logout);
  const navigate = useNavigate();

  const links = [
    { to: '/catalogo', label: 'Catálogo', roles: ['Vendedor', 'Gestor'] },
    { to: '/carrinho', label: 'Carrinho', roles: ['Vendedor', 'Gestor'] },
    { to: '/dashboard', label: 'Dashboard', roles: ['Vendedor', 'Gestor'] },
    { to: '/admin', label: 'Administração', roles: ['Gestor'] }
  ];

  return (
    <div className="flex min-h-screen flex-col">
      <header className="border-b bg-white shadow-sm">
        <div className="mx-auto flex max-w-6xl items-center justify-between px-6 py-4">
          <div>
            <h1 className="text-xl font-semibold text-primary-700">Projeto Revendedores</h1>
            <p className="text-sm text-slate-500">Gestão de catálogo, pedidos e relatórios</p>
          </div>
          {user ? (
            <div className="flex items-center gap-4">
              <span className="text-sm text-slate-600">
                {user.name} · {user.role}
              </span>
              <button
                onClick={() => {
                  logout();
                  navigate('/login');
                }}
                className="rounded-md bg-primary-600 px-3 py-1.5 text-sm font-medium text-white hover:bg-primary-700"
              >
                Sair
              </button>
            </div>
          ) : (
            <button
              onClick={() => navigate('/login')}
              className="rounded-md bg-primary-600 px-3 py-1.5 text-sm font-medium text-white hover:bg-primary-700"
            >
              Entrar
            </button>
          )}
        </div>
        <nav className="bg-primary-700 text-white">
          <div className="mx-auto flex max-w-6xl items-center gap-4 px-6 py-2 text-sm">
            {links
              .filter((link) => !user || link.roles.includes(user.role))
              .map((link) => (
                <NavLink
                  key={link.to}
                  to={link.to}
                  className={({ isActive }) =>
                    `rounded px-3 py-1 font-medium transition-colors ${isActive ? 'bg-white text-primary-700' : 'hover:bg-primary-600'}`
                  }
                >
                  {link.label}
                </NavLink>
              ))}
          </div>
        </nav>
      </header>

      <main className="mx-auto flex w-full max-w-6xl flex-1 flex-col gap-6 px-6 py-8">
        <Outlet />
      </main>

      <footer className="mt-auto border-t bg-white">
        <SyncStatusBanner />
        <div className="mx-auto max-w-6xl px-6 py-4 text-sm text-slate-500">
          <p>
            © {new Date().getFullYear()} Projeto Revendedores. Construído para operação offline-first.
          </p>
        </div>
      </footer>
    </div>
  );
}

export function PublicLayout() {
  return (
    <Fragment>
      <main className="flex min-h-screen items-center justify-center bg-gradient-to-br from-primary-50 to-primary-100">
        <Outlet />
      </main>
    </Fragment>
  );
}
