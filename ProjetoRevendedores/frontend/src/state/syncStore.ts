import { create } from 'zustand';

export type SyncStatus = 'idle' | 'syncing' | 'done' | 'error';

interface SyncState {
  status: SyncStatus;
  lastSync?: string;
  setStatus: (status: SyncStatus) => void;
  setLastSync: (isoDate?: string) => void;
}

export const useSyncStore = create<SyncState>((set) => ({
  status: 'idle',
  lastSync: undefined,
  setStatus: (status) => set({ status }),
  setLastSync: (isoDate) => set({ lastSync: isoDate })
}));
