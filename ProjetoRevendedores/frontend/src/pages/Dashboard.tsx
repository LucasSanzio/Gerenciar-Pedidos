import { useMemo, useState } from 'react';
import { useVendasPorDia } from '../features/relatorios/useVendasPorDia';
import { formatCurrencyFromCents } from '../utils/currency';

export default function DashboardPage() {
  const [data, setData] = useState(() => new Date().toISOString().slice(0, 10));
  const { data: relatorio, isLoading, isError, refetch } = useVendasPorDia(data);

  const totalDia = useMemo(
    () => (relatorio ?? []).reduce((acc, item) => acc + item.totalCentavos, 0),
    [relatorio]
  );

  return (
    <section className="space-y-6">
      <header className="flex flex-col gap-2">
        <h2 className="text-2xl font-semibold text-slate-800">Dashboard</h2>
        <p className="text-sm text-slate-500">
          Acompanhe as vendas do dia agrupadas por setor e monitore a performance da equipe.
        </p>
      </header>

      <div className="flex flex-wrap items-center gap-4">
        <label className="text-sm text-slate-600">
          Data do relatório
          <input
            type="date"
            value={data}
            onChange={(event) => setData(event.target.value)}
            className="ml-2 rounded-md border border-slate-200 px-3 py-1 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
          />
        </label>
        <button
          onClick={() => refetch()}
          className="rounded-md bg-primary-600 px-3 py-2 text-sm font-semibold text-white hover:bg-primary-700"
        >
          Atualizar
        </button>
      </div>

      {isLoading && <p className="text-sm text-slate-500">Carregando dados…</p>}
      {isError && <p className="text-sm text-red-500">Não foi possível carregar o relatório.</p>}

      {relatorio && (
        <div className="overflow-hidden rounded-lg border border-slate-200 bg-white shadow-sm">
          <table className="min-w-full divide-y divide-slate-200 text-sm">
            <thead className="bg-slate-50 text-left text-xs uppercase text-slate-500">
              <tr>
                <th className="px-4 py-2 font-medium">Setor</th>
                <th className="px-4 py-2 font-medium">Pedidos</th>
                <th className="px-4 py-2 font-medium">Total</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-200">
              {relatorio.map((linha) => (
                <tr key={linha.setorId}>
                  <td className="px-4 py-2 text-slate-700">{linha.setorNome}</td>
                  <td className="px-4 py-2 text-slate-500">{linha.pedidos}</td>
                  <td className="px-4 py-2 text-slate-500">{formatCurrencyFromCents(linha.totalCentavos)}</td>
                </tr>
              ))}
              <tr>
                <td className="px-4 py-3 text-right text-sm font-semibold text-slate-700" colSpan={2}>
                  Total do dia
                </td>
                <td className="px-4 py-3 text-sm font-semibold text-primary-700">
                  {formatCurrencyFromCents(totalDia)}
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      )}
    </section>
  );
}
