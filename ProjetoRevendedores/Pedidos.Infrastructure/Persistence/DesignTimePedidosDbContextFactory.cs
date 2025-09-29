using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pedidos.Infrastructure.Persistence;

/// <summary>
/// Factory utilizada pelo EF Core para criação de contexto em tempo de design.
/// </summary>
public class DesignTimePedidosDbContextFactory : IDesignTimeDbContextFactory<PedidosDbContext>
{
    public PedidosDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PedidosDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresPedidos;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new PedidosDbContext(optionsBuilder.Options);
    }
}
