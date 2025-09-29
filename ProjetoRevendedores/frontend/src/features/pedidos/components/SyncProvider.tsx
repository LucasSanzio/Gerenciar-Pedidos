import { useEffect, type PropsWithChildren } from 'react';
import { orderSyncQueue } from '../../../services/sync';
import { useSyncStore } from '../../../state/syncStore';

export function SyncProvider({ children }: PropsWithChildren) {
  const setStatus = useSyncStore((state) => state.setStatus);
  const setLastSync = useSyncStore((state) => state.setLastSync);

  useEffect(() => {
    const unsubscribe = orderSyncQueue.subscribe((status) => {
      setStatus(status);
      if (status === 'done') {
        setLastSync(new Date().toISOString());
      }
    });

    void orderSyncQueue.process();

    return () => {
      unsubscribe();
    };
  }, [setLastSync, setStatus]);

  return <>{children}</>;
}
