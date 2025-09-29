using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pedidos.Application.Pedidos.Commands.AddItem;
using Pedidos.Application.Pedidos.Commands.ApplyDiscount;
using Pedidos.Application.Pedidos.Commands.CloseOrder;
using Pedidos.Application.Pedidos.Commands.CreateOrder;
using Pedidos.Application.Pedidos.Commands.RemoveItem;
using Pedidos.Application.Pedidos.Models;

namespace Pedidos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PedidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria um novo pedido em rascunho.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PedidoDto>> CriarPedido([FromBody] CreateOrderRequest request)
    {
        var pedido = await _mediator.Send(new CreateOrderCommand(request.Observacao));
        return Created($"/api/pedidos/{pedido.Id}", pedido);
    }

    /// <summary>
    /// Adiciona um item ao pedido informado.
    /// </summary>
    [HttpPost("{id:guid}/itens")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PedidoDto>> AdicionarItem(Guid id, [FromBody] AddItemRequest request)
    {
        var pedido = await _mediator.Send(new AddItemCommand(id, request.ProdutoId, request.SetorId, request.SetorNome, request.NomeProduto, request.PrecoCentavos));
        return Ok(pedido);
    }

    /// <summary>
    /// Remove um item existente do pedido.
    /// </summary>
    [HttpDelete("{id:guid}/itens/{itemId:guid}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PedidoDto>> RemoverItem(Guid id, Guid itemId)
    {
        var pedido = await _mediator.Send(new RemoveItemCommand(id, itemId));
        return Ok(pedido);
    }

    /// <summary>
    /// Aplica desconto ao pedido.
    /// </summary>
    [HttpPost("{id:guid}/desconto")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PedidoDto>> AplicarDesconto(Guid id, [FromBody] ApplyDiscountRequest request)
    {
        var pedido = await _mediator.Send(new ApplyDiscountCommand(id, request.DescontoCentavos));
        return Ok(pedido);
    }

    /// <summary>
    /// Fecha o pedido impedindo novas alterações.
    /// </summary>
    [HttpPost("{id:guid}/fechar")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<PedidoDto>> FecharPedido(Guid id)
    {
        var pedido = await _mediator.Send(new CloseOrderCommand(id));
        return Ok(pedido);
    }

    public record CreateOrderRequest(string? Observacao);
    public record AddItemRequest(Guid ProdutoId, Guid SetorId, string SetorNome, string NomeProduto, int PrecoCentavos);
    public record ApplyDiscountRequest(int DescontoCentavos);
}
