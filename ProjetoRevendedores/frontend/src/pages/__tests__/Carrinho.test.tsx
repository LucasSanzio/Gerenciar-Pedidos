import { act, render, screen, within } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import CarrinhoPage from '../Carrinho';
import { useCartStore } from '../../state/cartStore';

describe('CarrinhoPage', () => {
  beforeEach(() => {
    act(() => {
      useCartStore.setState({
        items: [],
        observacao: '',
        descontoPercentual: 0,
        lastClosedOrder: undefined
      });
    });
  });

  afterEach(() => {
    act(() => {
      useCartStore.setState({
        items: [],
        observacao: '',
        descontoPercentual: 0,
        lastClosedOrder: undefined
      });
    });
  });

  it('permite atualizar itens e recalcula o total do carrinho', async () => {
    const user = userEvent.setup();
    const produto = {
      id: 'produto-1',
      nome: 'Camiseta Azul',
      precoCentavos: 1000,
      setorId: 'setor-1',
      setorNome: 'Masculino',
      ativo: true,
      iconeUrl: 'https://via.placeholder.com/48'
    } as const;

    act(() => {
      const store = useCartStore.getState();
      store.addItem(produto);
      store.addItem(produto);
    });

    render(<CarrinhoPage />);

    const subtotalRow = screen.getByText('Subtotal').parentElement!;
    expect(within(subtotalRow).getByText('R$ 20,00')).toBeInTheDocument();

    const totalRow = screen.getByText(/^Total$/).parentElement!;
    expect(within(totalRow).getByText('R$ 20,00')).toBeInTheDocument();

    const quantityInput = screen.getByDisplayValue('2') as HTMLInputElement;
    await user.clear(quantityInput);
    await user.type(quantityInput, '3');

    expect(await within(totalRow).findByText('R$ 30,00')).toBeInTheDocument();

    await user.click(screen.getByRole('button', { name: 'Remover' }));

    expect(
      await screen.findByText('Seu carrinho está vazio. Volte ao catálogo para adicionar produtos.')
    ).toBeInTheDocument();
  });
});
