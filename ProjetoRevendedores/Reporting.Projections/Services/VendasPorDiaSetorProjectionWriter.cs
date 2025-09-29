using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Reporting;
using Reporting.Projections.Entities;
using Reporting.Projections.Persistence;

namespace Reporting.Projections.Services;

/// <summary>
/// Serviço responsável por manter a projeção de vendas por dia e setor atualizada.
/// </summary>
public class VendasPorDiaSetorProjectionWriter : IVendasPorDiaSetorProjectionWriter
{
    private readonly ReportingDbContext _context;

    public VendasPorDiaSetorProjectionWriter(ReportingDbContext context)
    {
        _context = context;
    }

    public async Task RegistrarPedidoFechadoAsync(DateOnly dataFechamento, IReadOnlyCollection<PedidoItemSetorSnapshot> itens, int descontoTotalCentavos, CancellationToken cancellationToken)
    {
        if (itens is null)
        {
            throw new ArgumentNullException(nameof(itens));
        }

        if (itens.Count == 0)
        {
            return;
        }

        var grupos = itens
            .GroupBy(item => new { item.SetorId, Nome = item.SetorNome.Trim() })
            .Select(group => new
            {
                group.Key.SetorId,
                Nome = group.Key.Nome,
                QuantidadeItens = group.Count(),
                TotalCentavos = group.Sum(i => i.PrecoCentavos)
            })
            .ToList();

        if (grupos.Count == 0)
        {
            return;
        }

        var totalBruto = grupos.Sum(g => g.TotalCentavos);
        var descontoAplicado = Math.Clamp(descontoTotalCentavos, 0, totalBruto);

        var setorIds = grupos.Select(g => g.SetorId).ToList();

        var existentes = await _context.VendasPorDiaSetorViews
            .Where(v => v.Data == dataFechamento && setorIds.Contains(v.SetorId))
            .ToDictionaryAsync(v => v.SetorId, cancellationToken);

        var descontoDistribuido = 0;

        for (var index = 0; index < grupos.Count; index++)
        {
            var grupo = grupos[index];
            var descontoDoGrupo = 0;

            if (descontoAplicado > 0 && totalBruto > 0)
            {
                if (index == grupos.Count - 1)
                {
                    descontoDoGrupo = descontoAplicado - descontoDistribuido;
                }
                else
                {
                    descontoDoGrupo = (int)Math.Round((double)descontoAplicado * grupo.TotalCentavos / totalBruto, MidpointRounding.AwayFromZero);

                    if (descontoDoGrupo + descontoDistribuido > descontoAplicado)
                    {
                        descontoDoGrupo = descontoAplicado - descontoDistribuido;
                    }
                }

                descontoDoGrupo = Math.Min(descontoDoGrupo, grupo.TotalCentavos);
            }

            descontoDistribuido += descontoDoGrupo;

            var valorLiquido = Math.Max(grupo.TotalCentavos - descontoDoGrupo, 0);

            if (!existentes.TryGetValue(grupo.SetorId, out var projeção))
            {
                projeção = new VendasPorDiaSetorView(dataFechamento, grupo.SetorId, grupo.Nome);
                _context.VendasPorDiaSetorViews.Add(projeção);
                existentes.Add(grupo.SetorId, projeção);
            }
            else
            {
                projeção.AtualizarNome(grupo.Nome);
            }

            projeção.RegistrarVenda(valorLiquido, grupo.QuantidadeItens, contarPedido: true);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
