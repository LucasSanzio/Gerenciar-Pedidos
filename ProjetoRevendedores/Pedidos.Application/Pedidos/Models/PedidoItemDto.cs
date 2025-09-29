using Pedidos.Domain.Entities;

namespace Pedidos.Application.Pedidos.Models;

/// <summary>
/// Representa um item do pedido na camada de aplicação.
/// </summary>
public record PedidoItemDto(Guid Id, Guid ProdutoId, string NomeProduto, int PrecoCentavos)
{
    public static PedidoItemDto FromEntity(PedidoItem item) =>
        new(item.Id, item.ProdutoId, item.NomeProduto, item.PrecoCentavos);
}
