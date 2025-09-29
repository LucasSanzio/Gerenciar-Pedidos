using System;
using Pedidos.Domain.Entities;
using Xunit;

namespace Pedidos.Domain.Tests;

public class PedidoTests
{
    [Fact]
    public void Fechar_DeveFalhar_QuandoNaoPossuiItens()
    {
        var pedido = new Pedido("Teste sem itens");

        var exception = Assert.Throws<InvalidOperationException>(() => pedido.Fechar());

        Assert.Equal("Não é possível fechar um pedido sem itens.", exception.Message);
    }

    [Fact]
    public void Fechar_DeveAlterarStatus_QuandoPossuiItens()
    {
        var pedido = new Pedido();
        pedido.AdicionarItem(Guid.NewGuid(), Guid.NewGuid(), "Setor", "Produto", 1500);

        pedido.Fechar();

        Assert.Equal(PedidoStatus.Fechado, pedido.Status);
    }
}
