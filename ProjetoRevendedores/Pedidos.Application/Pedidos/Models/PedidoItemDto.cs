using Pedidos.Domain.Entities;

namespace Pedidos.Application.Pedidos.Models;

/// <summary>
/// Representa um item do pedido na camada de aplicação.
/// </summary>
public record PedidoItemDto(Guid Id, Guid ProdutoId, Guid SetorId, string SetorNome, string NomeProduto, int PrecoCentavos)
{
    public static PedidoItemDto FromEntity(PedidoItem item) =>
        new(item.Id, item.ProdutoId, item.SetorId, item.SetorNome, item.NomeProduto, item.PrecoCentavos);
}
