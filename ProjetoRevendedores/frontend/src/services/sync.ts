import type { AxiosError } from 'axios';
import { api } from './api';
import { getDb, type OfflineOrderEntity } from './db';
import type { SyncStatus } from '../state/syncStore';

export interface OrderItemPayload {
  produtoId: string;
  nome: string;
  iconeUrl: string;
  setorId: string;
  setorNome?: string;
  precoCentavos: number;
  quantidade: number;
}

export interface OrderPayload {
  numero: string;
  criadoEm: string;
  observacao?: string;
  descontoPercentual: number;
  totalCentavos: number;
  itens: OrderItemPayload[];
}

type Listener = (status: SyncStatus) => void;

function isNetworkError(error: unknown): boolean {
  const axiosError = error as AxiosError;
  return !axiosError || !axiosError.response;
}

class OrderSyncQueue {
  private listeners = new Set<Listener>();
  private status: SyncStatus = 'idle';
  private processing = false;

  constructor() {
    if (typeof window !== 'undefined') {
      window.addEventListener('online', () => {
        void this.process();
      });
    }
  }

  subscribe(listener: Listener): () => void {
    this.listeners.add(listener);
    listener(this.status);
    return () => {
      this.listeners.delete(listener);
    };
  }

  private setStatus(status: SyncStatus) {
    this.status = status;
    for (const listener of this.listeners) {
      listener(status);
    }
  }

  private async saveOffline(payload: OrderPayload) {
    const db = getDb();
    const entity: OfflineOrderEntity = {
      id: payload.numero,
      payload,
      createdAt: payload.criadoEm
    };
    await db.offlineOrders.put(entity);
  }

  async submit(payload: OrderPayload): Promise<void> {
    if (typeof window === 'undefined') {
      throw new Error('A fila offline está disponível apenas no navegador.');
    }

    if (!navigator.onLine) {
      await this.saveOffline(payload);
      this.setStatus('idle');
      return;
    }

    try {
      await api.post('/api/pedidos/fechar', payload);
      this.setStatus('done');
    } catch (error) {
      if (isNetworkError(error)) {
        await this.saveOffline(payload);
        this.setStatus('error');
      } else {
        throw error;
      }
    } finally {
      void this.process();
    }
  }

  async process(): Promise<void> {
    if (this.processing || typeof window === 'undefined') {
      return;
    }

    const db = getDb();
    const pending = await db.offlineOrders.orderBy('createdAt').toArray();
    if (!pending.length) {
      this.setStatus('done');
      return;
    }

    if (!navigator.onLine) {
      this.setStatus('idle');
      return;
    }

    this.processing = true;
    this.setStatus('syncing');

    for (const order of pending) {
      try {
        await api.post('/api/pedidos/fechar', order.payload as OrderPayload);
        await db.offlineOrders.delete(order.id);
      } catch (error) {
        if (isNetworkError(error)) {
          this.setStatus('error');
          this.processing = false;
          return;
        }

        console.error('Erro ao sincronizar pedido, descartando item problemático', error);
        await db.offlineOrders.delete(order.id);
      }
    }

    this.processing = false;
    this.setStatus('done');
  }
}

export const orderSyncQueue = new OrderSyncQueue();
