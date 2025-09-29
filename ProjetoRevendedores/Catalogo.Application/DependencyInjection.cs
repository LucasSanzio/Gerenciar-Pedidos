using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo.Application;

/// <summary>
/// Extensões para configurar serviços da camada de aplicação.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddCatalogoApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
