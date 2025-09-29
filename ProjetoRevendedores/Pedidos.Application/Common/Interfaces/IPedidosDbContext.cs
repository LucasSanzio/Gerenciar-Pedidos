using Microsoft.EntityFrameworkCore;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Common.Interfaces;

/// <summary>
/// Contrato utilizado pelos handlers para acessar o banco de pedidos.
/// </summary>
public interface IPedidosDbContext
{
    DbSet<Pedido> Pedidos { get; }
    DbSet<PedidoItem> PedidoItens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
