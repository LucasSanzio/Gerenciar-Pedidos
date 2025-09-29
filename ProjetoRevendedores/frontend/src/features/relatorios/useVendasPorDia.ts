import { useQuery } from '@tanstack/react-query';
import { api } from '../../services/api';

export interface VendasPorSetor {
  setorId: string;
  setorNome: string;
  totalCentavos: number;
  pedidos: number;
}

async function fetchVendasPorDia(data: string) {
  const response = await api.get<VendasPorSetor[]>('/api/relatorios/vendas', {
    params: { data }
  });
  return response.data;
}

export function useVendasPorDia(data: string) {
  return useQuery({
    queryKey: ['vendas-dia', data],
    queryFn: () => fetchVendasPorDia(data)
  });
}
