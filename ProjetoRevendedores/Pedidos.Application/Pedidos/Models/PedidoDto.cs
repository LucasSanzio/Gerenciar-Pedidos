using System.Collections.Generic;
using System.Linq;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Pedidos.Models;

/// <summary>
/// DTO principal de pedidos utilizado nas respostas da API.
/// </summary>
public record PedidoDto(
    Guid Id,
    string Status,
    string Observacao,
    int DescontoCentavos,
    int TotalCentavos,
    IReadOnlyCollection<PedidoItemDto> Itens)
{
    public static PedidoDto FromEntity(Pedido pedido)
    {
        var itens = pedido.Itens
            .Select(PedidoItemDto.FromEntity)
            .ToList()
            .AsReadOnly();

        return new PedidoDto(
            pedido.Id,
            pedido.Status.ToString(),
            pedido.Observacao,
            pedido.DescontoCentavos,
            pedido.TotalCentavos,
            itens);
    }
}
