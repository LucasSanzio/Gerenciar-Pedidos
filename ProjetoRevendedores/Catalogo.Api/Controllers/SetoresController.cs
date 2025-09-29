using Catalogo.Application.Setores.Commands.CreateSetor;
using Catalogo.Application.Setores.Commands.UpdateSetor;
using Catalogo.Application.Setores.Models;
using Catalogo.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Api.Controllers;

/// <summary>
/// Endpoints de gerenciamento de setores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SetoresController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CatalogoDbContext _context;

    public SetoresController(IMediator mediator, CatalogoDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Lista os setores cadastrados.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SetorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SetorDto>>> Get()
    {
        var setores = await _context.Setores.AsNoTracking()
            .OrderBy(s => s.Ordem)
            .Select(s => SetorDto.FromEntity(s))
            .ToListAsync();

        return Ok(setores);
    }

    /// <summary>
    /// Obtém um setor específico.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SetorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SetorDto>> GetById(Guid id)
    {
        var setor = await _context.Setores.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (setor is null)
        {
            return NotFound();
        }

        return Ok(SetorDto.FromEntity(setor));
    }

    /// <summary>
    /// Cria um novo setor.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SetorDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<SetorDto>> Post([FromBody] CreateSetorCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Atualiza um setor existente.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(SetorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SetorDto>> Put(Guid id, [FromBody] UpdateSetorCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(new { Message = "Id da rota diferente do corpo da requisição." });
        }

        var existe = await _context.Setores.AsNoTracking().AnyAsync(s => s.Id == id);
        if (!existe)
        {
            return NotFound();
        }

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Remove um setor.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var setor = await _context.Setores.Include(s => s.Produtos).FirstOrDefaultAsync(s => s.Id == id);
        if (setor is null)
        {
            return NotFound();
        }

        if (setor.Produtos.Any())
        {
            return BadRequest(new { Message = "Não é possível remover um setor que possui produtos vinculados." });
        }

        _context.Setores.Remove(setor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
