using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;
using Pedidos.Infrastructure.Persistence;
using Xunit;

namespace Pedidos.Infrastructure.Tests;

public class PedidosDbContextTests
{
    [Fact]
    public async Task DevePersistirPedidoComItensUsandoSqliteEmMemoria()
    {
        await using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<PedidosDbContext>()
            .UseSqlite(connection)
            .Options;

        await using (var arrangeContext = new PedidosDbContext(options))
        {
            await arrangeContext.Database.EnsureCreatedAsync();

            var pedido = new Pedido("Pedido de teste");
            pedido.AdicionarItem(Guid.NewGuid(), Guid.NewGuid(), "Masculino", "Camiseta", 5000);
            pedido.AdicionarItem(Guid.NewGuid(), Guid.NewGuid(), "Masculino", "Calça", 7000);
            pedido.AplicarDesconto(1000);
            pedido.Fechar();

            arrangeContext.Pedidos.Add(pedido);
            await arrangeContext.SaveChangesAsync();
        }

        await using (var assertContext = new PedidosDbContext(options))
        {
            var pedidoPersistido = await assertContext.Pedidos
                .Include(p => p.Itens)
                .SingleAsync();

            Assert.Equal(PedidoStatus.Fechado, pedidoPersistido.Status);
            Assert.Equal(2, pedidoPersistido.Itens.Count);
            Assert.Equal(11000, pedidoPersistido.TotalCentavos);
            Assert.All(pedidoPersistido.Itens, item => Assert.Equal(pedidoPersistido.Id, item.PedidoId));
            Assert.Contains(pedidoPersistido.Itens, item => item.NomeProduto == "Camiseta");
        }
    }
}
