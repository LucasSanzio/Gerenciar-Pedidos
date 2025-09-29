import { MinusIcon, PlusIcon } from '@heroicons/react/24/solid';
import { Produto } from '../../../state/catalogStore';
import { formatCurrencyFromCents } from '../../../utils/currency';

interface ProductCardProps {
  produto: Produto;
  quantidade: number;
  onIncrement: () => void;
  onDecrement: () => void;
}

export function ProductCard({ produto, quantidade, onIncrement, onDecrement }: ProductCardProps) {
  return (
    <div className="flex flex-col rounded-xl border border-slate-200 bg-white p-4 shadow-sm transition hover:shadow-md">
      <div className="flex items-start gap-3">
        <img
          src={produto.iconeUrl}
          alt={produto.nome}
          className="h-12 w-12 rounded-full border border-slate-200 object-cover"
          onError={(event) => {
            (event.target as HTMLImageElement).style.visibility = 'hidden';
          }}
        />
        <div className="flex-1">
          <h3 className="text-lg font-semibold text-slate-800">{produto.nome}</h3>
          <p className="text-sm text-slate-500">{formatCurrencyFromCents(produto.precoCentavos)}</p>
        </div>
      </div>

      <div className="mt-4 flex items-center justify-between">
        <span className="text-xs uppercase tracking-wide text-primary-600">{quantidade} no carrinho</span>
        <div className="flex items-center gap-2">
          <button
            onClick={onDecrement}
            disabled={quantidade === 0}
            className="rounded-full bg-slate-100 p-2 text-slate-600 hover:bg-slate-200 disabled:opacity-40"
            aria-label="Remover do carrinho"
          >
            <MinusIcon className="h-4 w-4" />
          </button>
          <button
            onClick={onIncrement}
            className="rounded-full bg-primary-600 p-2 text-white hover:bg-primary-700"
            aria-label="Adicionar ao carrinho"
          >
            <PlusIcon className="h-4 w-4" />
          </button>
        </div>
      </div>
    </div>
  );
}
