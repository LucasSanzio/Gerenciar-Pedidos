using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pedidos.Application.Reporting;
using Reporting.Projections.Persistence;
using Reporting.Projections.Services;

namespace Reporting.Projections;

/// <summary>
/// Extensões para configuração do módulo de projeções de relatórios.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddReportingProjections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Reporting")
            ?? "Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresReporting;Trusted_Connection=True;MultipleActiveResultSets=true";

        services.AddDbContext<ReportingDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IVendasPorDiaSetorProjectionWriter, VendasPorDiaSetorProjectionWriter>();

        return services;
    }
}
