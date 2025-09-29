import { useMemo } from 'react';
import { useSyncStore } from '../state/syncStore';
import clsx from 'clsx';

export function SyncStatusBanner() {
  const { status, lastSync } = useSyncStore((state) => ({
    status: state.status,
    lastSync: state.lastSync
  }));

  const message = useMemo(() => {
    switch (status) {
      case 'syncing':
        return 'Sincronizando pedidos…';
      case 'error':
        return 'Falha ao sincronizar pedidos. Tentaremos novamente em breve.';
      case 'done':
        return lastSync
          ? `Sincronização concluída às ${new Date(lastSync).toLocaleTimeString()}`
          : 'Sincronização concluída.';
      default:
        return lastSync
          ? `Última sincronização às ${new Date(lastSync).toLocaleTimeString()}`
          : undefined;
    }
  }, [lastSync, status]);

  if (!message) {
    return null;
  }

  return (
    <div
      className={clsx(
        'px-4 py-2 text-sm text-white transition-colors',
        status === 'error'
          ? 'bg-red-500'
          : status === 'syncing'
            ? 'bg-amber-500'
            : 'bg-emerald-600'
      )}
    >
      {message}
    </div>
  );
}
