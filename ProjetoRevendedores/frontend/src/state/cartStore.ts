import { create } from 'zustand';
import type { Produto } from './catalogStore';
import { orderSyncQueue, type OrderPayload } from '../services/sync';

export interface CartItem {
  produtoId: string;
  nome: string;
  iconeUrl: string;
  precoCentavos: number;
  setorId: string;
  setorNome?: string;
  quantidade: number;
}

export interface ClosedOrderSummary {
  numero: string;
  totalCentavos: number;
  criadoEm: string;
  itens: CartItem[];
  descontoPercentual: number;
  observacao?: string;
}

interface CartState {
  items: CartItem[];
  observacao: string;
  descontoPercentual: number;
  lastClosedOrder?: ClosedOrderSummary;
  addItem: (produto: Produto) => void;
  removeItem: (produtoId: string) => void;
  updateQuantity: (produtoId: string, quantidade: number) => void;
  setObservacao: (observacao: string) => void;
  setDescontoPercentual: (desconto: number) => void;
  clear: () => void;
  closeOrder: () => Promise<ClosedOrderSummary | null>;
}

function calcularTotal(items: CartItem[], descontoPercentual: number) {
  const subtotal = items.reduce(
    (acc, item) => acc + item.precoCentavos * item.quantidade,
    0
  );
  const desconto = Math.min(
    subtotal,
    Math.round((subtotal * Math.min(Math.max(descontoPercentual, 0), 100)) / 100)
  );
  return subtotal - desconto;
}

export const useCartStore = create<CartState>((set, get) => ({
  items: [],
  observacao: '',
  descontoPercentual: 0,
  lastClosedOrder: undefined,
  addItem: (produto) =>
    set((state) => {
      const existing = state.items.find((item) => item.produtoId === produto.id);
      if (existing) {
        return {
          items: state.items.map((item) =>
            item.produtoId === produto.id
              ? { ...item, quantidade: item.quantidade + 1 }
              : item
          )
        };
      }
      return {
        items: [
          ...state.items,
          {
            produtoId: produto.id,
            nome: produto.nome,
            iconeUrl: produto.iconeUrl,
            precoCentavos: produto.precoCentavos,
            setorId: produto.setorId,
            setorNome: produto.setorNome,
            quantidade: 1
          }
        ]
      };
    }),
  removeItem: (produtoId) =>
    set((state) => ({
      items: state.items.filter((item) => item.produtoId !== produtoId)
    })),
  updateQuantity: (produtoId, quantidade) =>
    set((state) => ({
      items: state.items
        .map((item) =>
          item.produtoId === produtoId ? { ...item, quantidade } : item
        )
        .filter((item) => item.quantidade > 0)
    })),
  setObservacao: (observacao) => set({ observacao }),
  setDescontoPercentual: (desconto) =>
    set({ descontoPercentual: Math.max(0, Math.min(desconto, 100)) }),
  clear: () => set({ items: [], observacao: '', descontoPercentual: 0 }),
  closeOrder: async () => {
    const state = get();
    if (!state.items.length) {
      return null;
    }

    const numero = `PED-${Date.now()}`;
    const totalCentavos = calcularTotal(state.items, state.descontoPercentual);
    const payload: OrderPayload = {
      numero,
      criadoEm: new Date().toISOString(),
      observacao: state.observacao,
      descontoPercentual: state.descontoPercentual,
      totalCentavos,
      itens: state.items.map((item) => ({
        produtoId: item.produtoId,
        nome: item.nome,
        iconeUrl: item.iconeUrl,
        setorId: item.setorId,
        setorNome: item.setorNome,
        precoCentavos: item.precoCentavos,
        quantidade: item.quantidade
      }))
    };

    await orderSyncQueue.submit(payload);

    const summary: ClosedOrderSummary = {
      numero,
      totalCentavos,
      criadoEm: payload.criadoEm,
      itens: state.items,
      descontoPercentual: state.descontoPercentual,
      observacao: state.observacao
    };

    set({
      lastClosedOrder: summary,
      items: [],
      observacao: '',
      descontoPercentual: 0
    });

    return summary;
  }
}));
