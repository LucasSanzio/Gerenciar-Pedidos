using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reporting.Projections.Persistence;

namespace Reporting.Api.Controllers;

[ApiController]
[Route("api/relatorios/vendas")]
public class RelatoriosController : ControllerBase
{
    private readonly ReportingDbContext _context;

    public RelatoriosController(ReportingDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retorna as vendas agregadas por setor para a data informada.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VendaPorSetorResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<VendaPorSetorResponse>>> ObterVendasPorSetor([FromQuery] string? data, CancellationToken cancellationToken)
    {
        var dataFiltro = DateOnly.FromDateTime(DateTime.UtcNow);

        if (!string.IsNullOrWhiteSpace(data))
        {
            if (!DateOnly.TryParseExact(data, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataFiltro))
            {
                return BadRequest(new { Message = "Formato de data inválido. Utilize YYYY-MM-DD." });
            }
        }

        var vendas = await _context.VendasPorDiaSetorViews
            .Where(v => v.Data == dataFiltro)
            .OrderBy(v => v.SetorNome)
            .Select(v => new VendaPorSetorResponse(
                v.Data.ToString("yyyy-MM-dd"),
                v.SetorId,
                v.SetorNome,
                v.TotalVendidoCentavos,
                v.QuantidadePedidos,
                v.QuantidadeItens))
            .ToListAsync(cancellationToken);

        return Ok(vendas);
    }

    public record VendaPorSetorResponse(
        string Data,
        Guid SetorId,
        string SetorNome,
        int TotalVendidoCentavos,
        int QuantidadePedidos,
        int QuantidadeItens);
}
