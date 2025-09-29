using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Pedidos.Application;

/// <summary>
/// Extensões para configuração da camada de aplicação de pedidos.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPedidosApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
