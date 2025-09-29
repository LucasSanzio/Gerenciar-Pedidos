import { FormEvent, useEffect, useMemo, useState } from 'react';
import { useCatalogStore, type Produto, type Setor } from '../state/catalogStore';
import { api } from '../services/api';
import { formatCurrencyFromCents } from '../utils/currency';

interface SetorFormState {
  nome: string;
  ordem: number;
  ativo: boolean;
}

interface ProdutoFormState {
  nome: string;
  precoCentavos: number;
  setorId: string;
  iconeUrl: string;
  ativo: boolean;
}

export default function AdminPage() {
  const { setores, produtos, reloadProducts, reloadSetores } = useCatalogStore((state) => ({
    setores: state.setores,
    produtos: state.produtos,
    reloadProducts: state.reloadProducts,
    reloadSetores: state.reloadSetores
  }));
  const [setorForm, setSetorForm] = useState<SetorFormState>({ nome: '', ordem: setores.length + 1, ativo: true });
  const [produtoForm, setProdutoForm] = useState<ProdutoFormState>({
    nome: '',
    precoCentavos: 0,
    setorId: setores[0]?.id ?? '',
    iconeUrl: '',
    ativo: true
  });
  const [feedback, setFeedback] = useState<string | undefined>();

  useEffect(() => {
    if (setores.length && !produtoForm.setorId) {
      setProdutoForm((prev) => ({ ...prev, setorId: setores[0].id }));
    }
  }, [produtoForm.setorId, setores]);

  useEffect(() => {
    setSetorForm((prev) => ({ ...prev, ordem: setores.length + 1 }));
  }, [setores.length]);

  useEffect(() => {
    if (!feedback) {
      return;
    }
    const timeout = window.setTimeout(() => setFeedback(undefined), 4000);
    return () => window.clearTimeout(timeout);
  }, [feedback]);

  const setoresOrdenados = useMemo(
    () => [...setores].sort((a, b) => a.ordem - b.ordem),
    [setores]
  );

  const handleSetorSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await api.post('/api/setores', setorForm);
      setFeedback('Setor criado com sucesso!');
      setSetorForm({ nome: '', ordem: setores.length + 2, ativo: true });
      await reloadSetores();
    } catch (error) {
      console.error(error);
      setFeedback('Erro ao criar setor.');
    }
  };

  const handleProdutoSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
      await api.post('/api/produtos', produtoForm);
      setFeedback('Produto criado com sucesso!');
      setProdutoForm({ nome: '', precoCentavos: 0, setorId: setores[0]?.id ?? '', iconeUrl: '', ativo: true });
      await reloadProducts();
    } catch (error) {
      console.error(error);
      setFeedback('Erro ao criar produto.');
    }
  };

  const toggleSetorStatus = async (setor: Setor) => {
    try {
      if (setor.ativo) {
        await api.post(`/api/setores/${setor.id}/desativar`);
      } else {
        await api.post(`/api/setores/${setor.id}/ativar`);
      }
      await reloadSetores();
    } catch (error) {
      console.error(error);
      setFeedback('Não foi possível atualizar o status do setor.');
    }
  };

  const toggleProdutoStatus = async (produto: Produto) => {
    try {
      if (produto.ativo) {
        await api.post(`/api/produtos/${produto.id}/desativar`);
      } else {
        await api.post(`/api/produtos/${produto.id}/ativar`);
      }
      await reloadProducts();
    } catch (error) {
      console.error(error);
      setFeedback('Não foi possível atualizar o status do produto.');
    }
  };

  return (
    <section className="space-y-8">
      <header className="space-y-2">
        <h2 className="text-2xl font-semibold text-slate-800">Administração</h2>
        <p className="text-sm text-slate-500">Gerencie setores e produtos disponíveis no catálogo.</p>
      </header>

      {feedback && <p className="rounded-md bg-slate-100 px-4 py-2 text-sm text-slate-600">{feedback}</p>}

      <div className="grid gap-8 lg:grid-cols-2">
        <div className="space-y-4">
          <h3 className="text-lg font-semibold text-slate-800">Novo setor</h3>
          <form onSubmit={handleSetorSubmit} className="space-y-3 rounded-lg border border-slate-200 bg-white p-4 shadow-sm">
            <label className="block text-sm text-slate-600">
              Nome
              <input
                type="text"
                value={setorForm.nome}
                onChange={(event) => setSetorForm((prev) => ({ ...prev, nome: event.target.value }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
                required
              />
            </label>
            <label className="block text-sm text-slate-600">
              Ordem
              <input
                type="number"
                value={setorForm.ordem}
                onChange={(event) => setSetorForm((prev) => ({ ...prev, ordem: Number(event.target.value) }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
                min={1}
              />
            </label>
            <label className="flex items-center gap-2 text-sm text-slate-600">
              <input
                type="checkbox"
                checked={setorForm.ativo}
                onChange={(event) => setSetorForm((prev) => ({ ...prev, ativo: event.target.checked }))}
              />
              Ativo
            </label>
            <button
              type="submit"
              className="w-full rounded-md bg-primary-600 px-3 py-2 text-sm font-semibold text-white hover:bg-primary-700"
            >
              Criar setor
            </button>
          </form>

          <div className="rounded-lg border border-slate-200 bg-white shadow-sm">
            <h4 className="border-b px-4 py-3 text-sm font-semibold text-slate-700">Setores cadastrados</h4>
            <ul className="divide-y divide-slate-200 text-sm">
              {setoresOrdenados.map((setor) => (
                <li key={setor.id} className="flex items-center justify-between px-4 py-3">
                  <div>
                    <p className="font-medium text-slate-800">{setor.nome}</p>
                    <p className="text-xs text-slate-500">Ordem: {setor.ordem}</p>
                  </div>
                  <button
                    onClick={() => toggleSetorStatus(setor)}
                    className="text-sm font-medium text-primary-600 hover:text-primary-700"
                  >
                    {setor.ativo ? 'Desativar' : 'Ativar'}
                  </button>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="space-y-4">
          <h3 className="text-lg font-semibold text-slate-800">Novo produto</h3>
          <form onSubmit={handleProdutoSubmit} className="space-y-3 rounded-lg border border-slate-200 bg-white p-4 shadow-sm">
            <label className="block text-sm text-slate-600">
              Nome
              <input
                type="text"
                value={produtoForm.nome}
                onChange={(event) => setProdutoForm((prev) => ({ ...prev, nome: event.target.value }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
                required
              />
            </label>
            <label className="block text-sm text-slate-600">
              Preço (centavos)
              <input
                type="number"
                min={0}
                value={produtoForm.precoCentavos}
                onChange={(event) => setProdutoForm((prev) => ({ ...prev, precoCentavos: Number(event.target.value) }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
                required
              />
            </label>
            <label className="block text-sm text-slate-600">
              Setor
              <select
                value={produtoForm.setorId}
                onChange={(event) => setProdutoForm((prev) => ({ ...prev, setorId: event.target.value }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
              >
                {setoresOrdenados.map((setor) => (
                  <option key={setor.id} value={setor.id}>
                    {setor.nome}
                  </option>
                ))}
              </select>
            </label>
            <label className="block text-sm text-slate-600">
              Ícone (URL)
              <input
                type="url"
                value={produtoForm.iconeUrl}
                onChange={(event) => setProdutoForm((prev) => ({ ...prev, iconeUrl: event.target.value }))}
                className="mt-1 w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
              />
            </label>
            <label className="flex items-center gap-2 text-sm text-slate-600">
              <input
                type="checkbox"
                checked={produtoForm.ativo}
                onChange={(event) => setProdutoForm((prev) => ({ ...prev, ativo: event.target.checked }))}
              />
              Ativo
            </label>
            <button
              type="submit"
              className="w-full rounded-md bg-primary-600 px-3 py-2 text-sm font-semibold text-white hover:bg-primary-700"
            >
              Criar produto
            </button>
          </form>

          <div className="overflow-hidden rounded-lg border border-slate-200 bg-white shadow-sm">
            <table className="min-w-full divide-y divide-slate-200 text-sm">
              <thead className="bg-slate-50 text-left text-xs uppercase text-slate-500">
                <tr>
                  <th className="px-4 py-2 font-medium">Produto</th>
                  <th className="px-4 py-2 font-medium">Setor</th>
                  <th className="px-4 py-2 font-medium">Preço</th>
                  <th className="px-4 py-2 font-medium">Status</th>
                  <th className="px-4 py-2" />
                </tr>
              </thead>
              <tbody className="divide-y divide-slate-200">
                {produtos.map((produto) => (
                  <tr key={produto.id}>
                    <td className="px-4 py-2 text-slate-700">{produto.nome}</td>
                    <td className="px-4 py-2 text-slate-500">
                      {setores.find((setor) => setor.id === produto.setorId)?.nome ?? '—'}
                    </td>
                    <td className="px-4 py-2 text-slate-500">{formatCurrencyFromCents(produto.precoCentavos)}</td>
                    <td className="px-4 py-2">
                      <span className={`rounded-full px-2 py-1 text-xs font-semibold ${
                        produto.ativo ? 'bg-emerald-100 text-emerald-700' : 'bg-slate-100 text-slate-500'
                      }`}>
                        {produto.ativo ? 'Ativo' : 'Inativo'}
                      </span>
                    </td>
                    <td className="px-4 py-2 text-right">
                      <button
                        onClick={() => toggleProdutoStatus(produto)}
                        className="text-sm font-medium text-primary-600 hover:text-primary-700"
                      >
                        {produto.ativo ? 'Desativar' : 'Ativar'}
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </section>
  );
}
