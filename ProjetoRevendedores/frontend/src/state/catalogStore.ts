import { create } from 'zustand';
import { api } from '../services/api';

export interface Setor {
  id: string;
  nome: string;
  ordem: number;
  ativo: boolean;
}

export interface Produto {
  id: string;
  nome: string;
  precoCentavos: number;
  setorId: string;
  setorNome?: string;
  ativo: boolean;
  iconeUrl: string;
}

interface CatalogState {
  setores: Setor[];
  produtos: Produto[];
  loading: boolean;
  error?: string;
  fetchCatalog: () => Promise<void>;
  reloadProducts: () => Promise<void>;
  reloadSetores: () => Promise<void>;
}

async function getSetores(): Promise<Setor[]> {
  const response = await api.get<Setor[]>('/api/setores');
  return response.data;
}

async function getProdutos(): Promise<Produto[]> {
  const response = await api.get<Produto[]>('/api/produtos');
  return response.data;
}

function hydrateProdutos(produtos: Produto[], setores: Setor[]): Produto[] {
  const setorMap = new Map(setores.map((setor) => [setor.id, setor.nome]));
  return produtos.map((produto) => ({
    ...produto,
    setorNome: setorMap.get(produto.setorId) ?? produto.setorNome
  }));
}

export const useCatalogStore = create<CatalogState>((set, get) => ({
  setores: [],
  produtos: [],
  loading: false,
  error: undefined,
  fetchCatalog: async () => {
    set({ loading: true, error: undefined });
    try {
      const [setores, produtos] = await Promise.all([getSetores(), getProdutos()]);
      set({ setores, produtos: hydrateProdutos(produtos, setores), loading: false });
    } catch (error) {
      console.error(error);
      set({ error: 'Não foi possível carregar o catálogo.', loading: false });
    }
  },
  reloadProducts: async () => {
    try {
      const produtos = await getProdutos();
      const setores = get().setores;
      set({ produtos: hydrateProdutos(produtos, setores) });
    } catch (error) {
      console.error(error);
      set({ error: 'Não foi possível carregar os produtos.' });
    }
  },
  reloadSetores: async () => {
    try {
      const setores = await getSetores();
      const produtos = hydrateProdutos(get().produtos, setores);
      set({ setores, produtos });
    } catch (error) {
      console.error(error);
      set({ error: 'Não foi possível carregar os setores.' });
    }
  }
}));
