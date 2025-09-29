using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Persistence;

/// <summary>
/// DbContext responsável por persistir pedidos e itens.
/// </summary>
public class PedidosDbContext : DbContext, IPedidosDbContext
{
    public PedidosDbContext(DbContextOptions<PedidosDbContext> options) : base(options)
    {
    }

    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PedidosDbContext).Assembly);
    }
}
