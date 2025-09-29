import Dexie, { type Table } from 'dexie';

export interface OfflineOrderEntity {
  id: string;
  payload: unknown;
  createdAt: string;
}

class SyncDatabase extends Dexie {
  offlineOrders!: Table<OfflineOrderEntity, string>;

  constructor() {
    super('ProjetoRevendedoresSync');
    this.version(1).stores({
      offlineOrders: '&id, createdAt'
    });
  }
}

let dbInstance: SyncDatabase | null = null;

export function getDb(): SyncDatabase {
  if (dbInstance) {
    return dbInstance;
  }
  if (typeof window === 'undefined') {
    throw new Error('IndexedDB não está disponível no ambiente atual');
  }
  dbInstance = new SyncDatabase();
  return dbInstance;
}
