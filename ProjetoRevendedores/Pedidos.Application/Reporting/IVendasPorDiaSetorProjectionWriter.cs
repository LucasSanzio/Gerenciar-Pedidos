namespace Pedidos.Application.Reporting;

/// <summary>
/// Representa um snapshot de item utilizado para atualizar projeções por setor.
/// </summary>
/// <param name="SetorId">Identificador do setor associado ao item.</param>
/// <param name="SetorNome">Nome do setor no momento da venda.</param>
/// <param name="PrecoCentavos">Valor do item em centavos.</param>
public record PedidoItemSetorSnapshot(Guid SetorId, string SetorNome, int PrecoCentavos);

/// <summary>
/// Contrato responsável por atualizar a projeção de vendas por dia e setor.
/// </summary>
public interface IVendasPorDiaSetorProjectionWriter
{
    Task RegistrarPedidoFechadoAsync(DateOnly dataFechamento, IReadOnlyCollection<PedidoItemSetorSnapshot> itens, int descontoTotalCentavos, CancellationToken cancellationToken);
}
