namespace Pedidos.Domain.Entities;

/// <summary>
/// Representa um item adicionado ao pedido com snapshot de preço e nome no momento da venda.
/// </summary>
public class PedidoItem
{
    /// <summary>
    /// Identificador único do item.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identificador do pedido ao qual o item pertence.
    /// </summary>
    public Guid PedidoId { get; private set; }

    /// <summary>
    /// Identificador do produto original.
    /// </summary>
    public Guid ProdutoId { get; private set; }

    /// <summary>
    /// Nome do produto no momento da venda.
    /// </summary>
    public string NomeProduto { get; private set; }

    /// <summary>
    /// Preço do produto em centavos no momento da venda.
    /// </summary>
    public int PrecoCentavos { get; private set; }

    /// <summary>
    /// Navegação inversa para o pedido.
    /// </summary>
    public Pedido? Pedido { get; private set; }

    private PedidoItem()
    {
        NomeProduto = string.Empty;
    }

    public PedidoItem(Guid produtoId, string nomeProduto, int precoCentavos)
        : this()
    {
        if (produtoId == Guid.Empty)
        {
            throw new ArgumentException("O identificador do produto é obrigatório.", nameof(produtoId));
        }

        if (string.IsNullOrWhiteSpace(nomeProduto))
        {
            throw new ArgumentException("O nome do produto é obrigatório.", nameof(nomeProduto));
        }

        if (precoCentavos < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(precoCentavos), "O preço do item deve ser maior ou igual a zero.");
        }

        Id = Guid.NewGuid();
        ProdutoId = produtoId;
        NomeProduto = nomeProduto.Trim();
        PrecoCentavos = precoCentavos;
    }

    internal void AtribuirPedido(Guid pedidoId)
    {
        PedidoId = pedidoId;
    }
}
