using Catalogo.Application.Common.Interfaces;
using Catalogo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Infrastructure;

/// <summary>
/// Extensões para configuração da camada de infraestrutura.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddCatalogoInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Catalogo")
            ?? "Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresCatalogo;Trusted_Connection=True;MultipleActiveResultSets=true";

        services.AddDbContext<CatalogoDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ICatalogoDbContext>(sp => sp.GetRequiredService<CatalogoDbContext>());

        return services;
    }
}
