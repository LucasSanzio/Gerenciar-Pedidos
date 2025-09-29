using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Infrastructure.Persistence;

namespace Pedidos.Infrastructure;

/// <summary>
/// Métodos de extensão para configurar serviços de infraestrutura de pedidos.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPedidosInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Pedidos")
            ?? "Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresPedidos;Trusted_Connection=True;MultipleActiveResultSets=true";

        services.AddDbContext<PedidosDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IPedidosDbContext>(sp => sp.GetRequiredService<PedidosDbContext>());

        return services;
    }
}
