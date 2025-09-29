import { test, expect } from '@playwright/test';

const setoresMock = [
  { id: 'setor-1', nome: 'Masculino', ordem: 1, ativo: true },
  { id: 'setor-2', nome: 'Feminino', ordem: 2, ativo: true }
];

const produtosMock = [
  {
    id: 'produto-1',
    nome: 'Camiseta Azul',
    precoCentavos: 2500,
    setorId: 'setor-1',
    setorNome: 'Masculino',
    ativo: true,
    iconeUrl: 'https://via.placeholder.com/64'
  }
];

const relatorioMock = [
  {
    setorId: 'setor-1',
    setorNome: 'Masculino',
    totalCentavos: 2500,
    pedidos: 1
  }
];

test.beforeEach(async ({ page }) => {
  await page.route('**/api/setores', async (route) => {
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify(setoresMock) });
  });

  await page.route('**/api/produtos', async (route) => {
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify(produtosMock) });
  });

  await page.route('**/api/pedidos/fechar', async (route) => {
    expect(route.request().method()).toBe('POST');
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify({ sucesso: true }) });
  });

  await page.route('**/api/relatorios/vendas*', async (route) => {
    await route.fulfill({ status: 200, contentType: 'application/json', body: JSON.stringify(relatorioMock) });
  });
});

test('fluxo completo de login até relatório de vendas', async ({ page }) => {
  await page.goto('/login');
  await page.fill('input[type="text"]', 'Maria Vendedora');
  await page.selectOption('select', 'Vendedor');
  await page.click('text=Entrar');

  await expect(page).toHaveURL(/catalogo/);
  await expect(page.getByRole('heading', { name: 'Catálogo' })).toBeVisible();

  await page.getByRole('button', { name: 'Adicionar ao carrinho' }).click();
  await page.getByRole('link', { name: 'Carrinho' }).click();

  await expect(page).toHaveURL(/carrinho/);
  await expect(page.getByText('Carrinho')).toBeVisible();
  await expect(page.getByText('R$ 25,00')).toBeVisible();

  const fecharPedido = page.getByRole('button', { name: 'Fechar pedido' });
  await Promise.all([
    page.waitForResponse('**/api/pedidos/fechar'),
    fecharPedido.click()
  ]);

  await expect(page.getByText('Pedido confirmado')).toBeVisible();

  await page.getByRole('link', { name: 'Dashboard' }).click();
  await expect(page).toHaveURL(/dashboard/);
  await expect(page.getByRole('table')).toBeVisible();
  await expect(page.getByRole('cell', { name: 'Masculino' })).toBeVisible();
  await expect(page.getByRole('cell', { name: 'R$ 25,00' })).toBeVisible();
});
