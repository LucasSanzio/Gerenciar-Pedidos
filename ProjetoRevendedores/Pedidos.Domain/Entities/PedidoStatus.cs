namespace Pedidos.Domain.Entities;

/// <summary>
/// Representa o status atual do pedido.
/// </summary>
public enum PedidoStatus
{
    /// <summary>
    /// Pedido em fase de montagem.
    /// </summary>
    Rascunho = 0,

    /// <summary>
    /// Pedido finalizado.
    /// </summary>
    Fechado = 1
}
