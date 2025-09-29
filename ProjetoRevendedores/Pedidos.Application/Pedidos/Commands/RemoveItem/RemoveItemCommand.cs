using System.Linq;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Application.Pedidos.Models;

namespace Pedidos.Application.Pedidos.Commands.RemoveItem;

/// <summary>
/// Command responsável por remover um item do pedido.
/// </summary>
public record RemoveItemCommand(Guid PedidoId, Guid ItemId) : IRequest<PedidoDto>;

/// <summary>
/// Validador do command de remoção de item.
/// </summary>
public class RemoveItemCommandValidator : AbstractValidator<RemoveItemCommand>
{
    public RemoveItemCommandValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O pedido é obrigatório.");

        RuleFor(x => x.ItemId)
            .NotEmpty().WithMessage("O item é obrigatório.");
    }
}

/// <summary>
/// Handler responsável por remover o item do pedido.
/// </summary>
public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, PedidoDto>
{
    private readonly IPedidosDbContext _context;
    private readonly IValidator<RemoveItemCommand> _validator;

    public RemoveItemCommandHandler(IPedidosDbContext context, IValidator<RemoveItemCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<PedidoDto> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == request.PedidoId, cancellationToken);

        if (pedido is null)
        {
            throw new ValidationException("Pedido não encontrado.");
        }

        var itemExiste = pedido.Itens.Any(i => i.Id == request.ItemId);
        if (!itemExiste)
        {
            throw new ValidationException("Item não encontrado no pedido.");
        }

        try
        {
            pedido.RemoverItem(request.ItemId);
        }
        catch (InvalidOperationException ex)
        {
            throw new ValidationException(ex.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return PedidoDto.FromEntity(pedido);
    }
}
