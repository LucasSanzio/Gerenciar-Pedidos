import clsx from 'clsx';
import { Setor } from '../../../state/catalogStore';

interface SetorTabsProps {
  setores: Setor[];
  ativo?: string;
  onSelect: (setorId?: string) => void;
}

export function SetorTabs({ setores, ativo, onSelect }: SetorTabsProps) {
  return (
    <div className="flex flex-wrap gap-2">
      <button
        onClick={() => onSelect(undefined)}
        className={clsx(
          'rounded-full border px-4 py-2 text-sm font-medium transition',
          !ativo
            ? 'border-primary-600 bg-primary-600 text-white'
            : 'border-slate-200 bg-white text-slate-600 hover:border-primary-200 hover:text-primary-600'
        )}
      >
        Todos
      </button>
      {setores.map((setor) => (
        <button
          key={setor.id}
          onClick={() => onSelect(setor.id)}
          className={clsx(
            'rounded-full border px-4 py-2 text-sm font-medium transition',
            ativo === setor.id
              ? 'border-primary-600 bg-primary-600 text-white'
              : 'border-slate-200 bg-white text-slate-600 hover:border-primary-200 hover:text-primary-600'
          )}
        >
          {setor.nome}
        </button>
      ))}
    </div>
  );
}
