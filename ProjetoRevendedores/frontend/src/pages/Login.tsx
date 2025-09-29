import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore, type UserRole } from '../state/authStore';

export default function LoginPage() {
  const [role, setRole] = useState<UserRole>('Vendedor');
  const [name, setName] = useState('');
  const login = useAuthStore((state) => state.login);
  const navigate = useNavigate();

  const handleSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    login(role, name || undefined);
    navigate('/catalogo');
  };

  return (
    <div className="w-full max-w-md rounded-xl bg-white p-8 shadow-xl">
      <h2 className="text-2xl font-semibold text-primary-700">Bem-vindo</h2>
      <p className="mt-2 text-sm text-slate-500">
        Escolha o seu perfil para acessar o Projeto Revendedores.
      </p>

      <form onSubmit={handleSubmit} className="mt-6 space-y-4">
        <div>
          <label className="block text-sm font-medium text-slate-600">Nome</label>
          <input
            type="text"
            value={name}
            onChange={(event) => setName(event.target.value)}
            placeholder="Digite seu nome"
            className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
          />
        </div>

        <div>
          <label className="block text-sm font-medium text-slate-600">Perfil</label>
          <select
            value={role}
            onChange={(event) => setRole(event.target.value as UserRole)}
            className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
          >
            <option value="Vendedor">Vendedor</option>
            <option value="Gestor">Gestor</option>
          </select>
        </div>

        <button
          type="submit"
          className="w-full rounded-md bg-primary-600 px-3 py-2 font-semibold text-white transition hover:bg-primary-700"
        >
          Entrar
        </button>
      </form>

      <p className="mt-6 text-xs text-slate-400">
        Este ambiente utiliza autenticação fictícia e sincronização offline para pedidos.
      </p>
    </div>
  );
}
