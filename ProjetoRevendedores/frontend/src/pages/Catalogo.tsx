import { useEffect, useMemo, useState } from 'react';
import { useCatalogStore } from '../state/catalogStore';
import { SetorTabs } from '../features/catalogo/components/SetorTabs';
import { ProductCard } from '../features/catalogo/components/ProductCard';
import { useCartStore } from '../state/cartStore';

export default function CatalogoPage() {
  const { setores, produtos, loading, error, fetchCatalog } = useCatalogStore((state) => ({
    setores: state.setores.filter((setor) => setor.ativo).sort((a, b) => a.ordem - b.ordem),
    produtos: state.produtos.filter((produto) => produto.ativo),
    loading: state.loading,
    error: state.error,
    fetchCatalog: state.fetchCatalog
  }));
  const addItem = useCartStore((state) => state.addItem);
  const removeItem = useCartStore((state) => state.removeItem);
  const updateQuantity = useCartStore((state) => state.updateQuantity);
  const cartItems = useCartStore((state) => state.items);
  const [setorSelecionado, setSetorSelecionado] = useState<string | undefined>();

  useEffect(() => {
    if (!setores.length || !produtos.length) {
      void fetchCatalog();
    }
  }, [fetchCatalog, produtos.length, setores.length]);

  const produtosFiltrados = useMemo(() => {
    if (!setorSelecionado) {
      return produtos;
    }
    return produtos.filter((produto) => produto.setorId === setorSelecionado);
  }, [produtos, setorSelecionado]);

  return (
    <section className="space-y-6">
      <header className="flex flex-col gap-2">
        <h2 className="text-2xl font-semibold text-slate-800">Catálogo</h2>
        <p className="text-sm text-slate-500">
          Explore os produtos por setor e adicione rapidamente ao seu carrinho.
        </p>
      </header>

      {error && <p className="rounded-md bg-red-100 px-4 py-2 text-sm text-red-700">{error}</p>}

      <SetorTabs setores={setores} ativo={setorSelecionado} onSelect={setSetorSelecionado} />

      {loading ? (
        <p className="text-sm text-slate-500">Carregando catálogo…</p>
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {produtosFiltrados.map((produto) => {
            const item = cartItems.find((cartItem) => cartItem.produtoId === produto.id);
            return (
              <ProductCard
                key={produto.id}
                produto={produto}
                quantidade={item?.quantidade ?? 0}
                onIncrement={() => addItem(produto)}
                onDecrement={() => {
                  if (item) {
                    if (item.quantidade === 1) {
                      removeItem(produto.id);
                    } else {
                      updateQuantity(produto.id, item.quantidade - 1);
                    }
                  }
                }}
              />
            );
          })}
        </div>
      )}
    </section>
  );
}
