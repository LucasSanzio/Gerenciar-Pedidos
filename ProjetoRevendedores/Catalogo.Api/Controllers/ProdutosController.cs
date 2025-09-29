using Catalogo.Application.Produtos.Commands.ActivateProduct;
using Catalogo.Application.Produtos.Commands.CreateProduct;
using Catalogo.Application.Produtos.Commands.DeactivateProduct;
using Catalogo.Application.Produtos.Commands.UpdateProduct;
using Catalogo.Application.Produtos.Models;
using Catalogo.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers;

/// <summary>
/// Endpoints de gerenciamento de produtos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CatalogoDbContext _context;

    public ProdutosController(IMediator mediator, CatalogoDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Lista todos os produtos cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProdutoDto>>> Get([FromQuery] Guid? setorId, [FromQuery] bool? ativo)
    {
        var query = _context.Produtos.AsNoTracking();

        if (setorId.HasValue)
        {
            query = query.Where(p => p.SetorId == setorId.Value);
        }

        if (ativo.HasValue)
        {
            query = query.Where(p => p.Ativo == ativo.Value);
        }

        var produtos = await query
            .OrderBy(p => p.Nome)
            .Select(p => ProdutoDto.FromEntity(p))
            .ToListAsync();

        return Ok(produtos);
    }

    /// <summary>
    /// Obtém um produto específico pelo identificador.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDto>> GetById(Guid id)
    {
        var produto = await _context.Produtos.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (produto is null)
        {
            return NotFound();
        }

        return Ok(ProdutoDto.FromEntity(produto));
    }

    /// <summary>
    /// Cria um novo produto.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ProdutoDto>> Post([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Atualiza as informações de um produto.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoDto>> Put(Guid id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { Message = "Id da rota diferente do corpo da requisição." });
        }

        var existe = await _context.Produtos.AsNoTracking().AnyAsync(p => p.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Ativa um produto.
    /// </summary>
    [HttpPut("{id:guid}/ativar")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProdutoDto>> Ativar(Guid id)
    {
        var result = await _mediator.Send(new ActivateProductCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Desativa um produto.
    /// </summary>
    [HttpPut("{id:guid}/desativar")]
    [ProducesResponseType(typeof(ProdutoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProdutoDto>> Desativar(Guid id)
    {
        var result = await _mediator.Send(new DeactivateProductCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Remove um produto do catálogo.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
        if (produto is null)
        {
            return NotFound();
        }

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
