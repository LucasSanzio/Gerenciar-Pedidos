import { useMemo, useState } from 'react';
import { useCartStore } from '../state/cartStore';
import { formatCurrencyFromCents } from '../utils/currency';

export default function CarrinhoPage() {
  const { items, observacao, descontoPercentual, setObservacao, setDescontoPercentual, updateQuantity, removeItem, closeOrder, lastClosedOrder } =
    useCartStore((state) => ({
      items: state.items,
      observacao: state.observacao,
      descontoPercentual: state.descontoPercentual,
      setObservacao: state.setObservacao,
      setDescontoPercentual: state.setDescontoPercentual,
      updateQuantity: state.updateQuantity,
      removeItem: state.removeItem,
      closeOrder: state.closeOrder,
      lastClosedOrder: state.lastClosedOrder
    }));
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | undefined>();

  const subtotal = useMemo(
    () => items.reduce((acc, item) => acc + item.precoCentavos * item.quantidade, 0),
    [items]
  );

  const total = useMemo(() => {
    const desconto = Math.min(subtotal, Math.round((subtotal * descontoPercentual) / 100));
    return subtotal - desconto;
  }, [descontoPercentual, subtotal]);

  const handleFecharPedido = async () => {
    setIsSubmitting(true);
    setError(undefined);
    try {
      await closeOrder();
    } catch (err) {
      console.error(err);
      setError('Não foi possível fechar o pedido. Verifique os dados e tente novamente.');
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <section className="space-y-6">
      <header className="flex flex-col gap-2">
        <h2 className="text-2xl font-semibold text-slate-800">Carrinho</h2>
        <p className="text-sm text-slate-500">
          Revise os itens, adicione observações e confirme o fechamento do pedido.
        </p>
      </header>

      {items.length === 0 ? (
        <p className="rounded-md bg-slate-100 px-4 py-3 text-sm text-slate-600">
          Seu carrinho está vazio. Volte ao catálogo para adicionar produtos.
        </p>
      ) : (
        <div className="space-y-4">
          <ul className="divide-y divide-slate-200 overflow-hidden rounded-lg border border-slate-200 bg-white shadow-sm">
            {items.map((item) => (
              <li key={item.produtoId} className="flex items-center justify-between gap-4 px-4 py-3">
                <div>
                  <p className="font-medium text-slate-800">{item.nome}</p>
                  <p className="text-xs text-slate-500">{item.setorNome}</p>
                  <p className="text-sm text-primary-600">{formatCurrencyFromCents(item.precoCentavos)}</p>
                </div>
                <div className="flex items-center gap-2">
                  <input
                    type="number"
                    min={1}
                    value={item.quantidade}
                    onChange={(event) => {
                      const value = Number(event.target.value);
                      updateQuantity(
                        item.produtoId,
                        Number.isNaN(value) ? 1 : Math.max(1, value)
                      );
                    }}
                    className="w-16 rounded-md border border-slate-200 px-2 py-1 text-center focus:border-primary-500 focus:outline-none"
                  />
                  <button
                    onClick={() => removeItem(item.produtoId)}
                    className="text-sm font-medium text-red-500 hover:text-red-600"
                  >
                    Remover
                  </button>
                </div>
              </li>
            ))}
          </ul>

          <div className="grid gap-4 sm:grid-cols-2">
            <label className="flex flex-col gap-2 text-sm text-slate-600">
              Observação
              <textarea
                value={observacao}
                onChange={(event) => setObservacao(event.target.value)}
                rows={4}
                className="w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
              />
            </label>

            <label className="flex flex-col gap-2 text-sm text-slate-600">
              Desconto (%)
              <input
                type="number"
                min={0}
                max={100}
                value={descontoPercentual}
                onChange={(event) => setDescontoPercentual(Number(event.target.value))}
                className="w-full rounded-md border border-slate-200 px-3 py-2 focus:border-primary-500 focus:outline-none focus:ring-2 focus:ring-primary-200"
              />
              <span className="text-xs text-slate-400">
                O desconto é aplicado proporcionalmente aos itens do pedido.
              </span>
            </label>
          </div>

          <div className="rounded-lg border border-slate-200 bg-white p-4 shadow-sm">
            <dl className="space-y-2 text-sm text-slate-600">
              <div className="flex justify-between">
                <dt>Subtotal</dt>
                <dd>{formatCurrencyFromCents(subtotal)}</dd>
              </div>
              <div className="flex justify-between">
                <dt>Desconto</dt>
                <dd>-{formatCurrencyFromCents(subtotal - total)}</dd>
              </div>
              <div className="flex justify-between text-base font-semibold text-primary-700">
                <dt>Total</dt>
                <dd>{formatCurrencyFromCents(total)}</dd>
              </div>
            </dl>
            <button
              onClick={handleFecharPedido}
              disabled={isSubmitting}
              className="mt-4 w-full rounded-md bg-primary-600 px-4 py-2 font-semibold text-white transition hover:bg-primary-700"
            >
              {isSubmitting ? 'Sincronizando…' : 'Fechar pedido'}
            </button>
            {error && <p className="mt-2 text-sm text-red-500">{error}</p>}
          </div>
        </div>
      )}

      {lastClosedOrder && (
        <div className="rounded-lg border border-emerald-200 bg-emerald-50 p-4">
          <h3 className="text-lg font-semibold text-emerald-700">Pedido confirmado</h3>
          <p className="text-sm text-emerald-600">
            Número do pedido: <span className="font-mono">{lastClosedOrder.numero}</span>
          </p>
          <p className="text-sm text-emerald-600">
            Total: {formatCurrencyFromCents(lastClosedOrder.totalCentavos)} · Criado em{' '}
            {new Date(lastClosedOrder.criadoEm).toLocaleString('pt-BR')}
          </p>
          <ul className="mt-3 space-y-1 text-sm text-emerald-700">
            {lastClosedOrder.itens.map((item) => (
              <li key={item.produtoId}>
                {item.quantidade}× {item.nome}
              </li>
            ))}
          </ul>
        </div>
      )}
    </section>
  );
}
