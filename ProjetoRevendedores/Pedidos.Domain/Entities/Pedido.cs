using System.Collections.Generic;
using System.Linq;

namespace Pedidos.Domain.Entities;

/// <summary>
/// Representa um pedido realizado no sistema.
/// </summary>
public class Pedido
{
    /// <summary>
    /// Identificador único do pedido.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Status atual do pedido.
    /// </summary>
    public PedidoStatus Status { get; private set; }

    /// <summary>
    /// Observações informadas pelo vendedor.
    /// </summary>
    public string Observacao { get; private set; }

    /// <summary>
    /// Valor do desconto aplicado em centavos.
    /// </summary>
    public int DescontoCentavos { get; private set; }

    /// <summary>
    /// Valor total do pedido em centavos após descontos.
    /// </summary>
    public int TotalCentavos { get; private set; }

    /// <summary>
    /// Lista de itens vinculados ao pedido.
    /// </summary>
    public ICollection<PedidoItem> Itens { get; private set; }

    private Pedido()
    {
        Observacao = string.Empty;
        Itens = new List<PedidoItem>();
        Status = PedidoStatus.Rascunho;
    }

    public Pedido(string? observacao = null)
        : this()
    {
        Id = Guid.NewGuid();
        AtualizarObservacao(observacao);
        RecalcularTotal();
    }

    /// <summary>
    /// Atualiza a observação do pedido.
    /// </summary>
    public void AtualizarObservacao(string? observacao)
    {
        Observacao = observacao?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Adiciona um item ao pedido.
    /// </summary>
    public PedidoItem AdicionarItem(Guid produtoId, Guid setorId, string setorNome, string nomeProduto, int precoCentavos)
    {
        GarantirRascunho();

        var item = new PedidoItem(produtoId, setorId, setorNome, nomeProduto, precoCentavos);
        item.AtribuirPedido(Id);

        Itens.Add(item);
        RecalcularTotal();

        return item;
    }

    /// <summary>
    /// Remove um item existente do pedido.
    /// </summary>
    public void RemoverItem(Guid itemId)
    {
        GarantirRascunho();

        var item = Itens.FirstOrDefault(i => i.Id == itemId)
            ?? throw new InvalidOperationException("Item não encontrado no pedido.");

        Itens.Remove(item);
        RecalcularTotal();
    }

    /// <summary>
    /// Aplica desconto em centavos sobre o subtotal do pedido.
    /// </summary>
    public void AplicarDesconto(int descontoCentavos)
    {
        GarantirRascunho();

        if (descontoCentavos < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(descontoCentavos), "O desconto deve ser maior ou igual a zero.");
        }

        var subtotal = Itens.Sum(i => i.PrecoCentavos);
        if (descontoCentavos > subtotal)
        {
            throw new InvalidOperationException("O desconto não pode ser maior que o subtotal do pedido.");
        }

        DescontoCentavos = descontoCentavos;
        RecalcularTotal();
    }

    /// <summary>
    /// Finaliza o pedido, impedindo novas alterações.
    /// </summary>
    public void Fechar()
    {
        GarantirRascunho();

        if (!Itens.Any())
        {
            throw new InvalidOperationException("Não é possível fechar um pedido sem itens.");
        }

        Status = PedidoStatus.Fechado;
    }

    private void RecalcularTotal()
    {
        var subtotal = Itens.Sum(i => i.PrecoCentavos);
        if (DescontoCentavos > subtotal)
        {
            DescontoCentavos = subtotal;
        }

        TotalCentavos = subtotal - DescontoCentavos;
    }

    private void GarantirRascunho()
    {
        if (Status != PedidoStatus.Rascunho)
        {
            throw new InvalidOperationException("Apenas pedidos em rascunho podem ser alterados.");
        }
    }
}
