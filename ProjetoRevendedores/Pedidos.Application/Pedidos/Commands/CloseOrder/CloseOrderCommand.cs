using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Application.Pedidos.Models;

namespace Pedidos.Application.Pedidos.Commands.CloseOrder;

/// <summary>
/// Command responsável por fechar um pedido.
/// </summary>
public record CloseOrderCommand(Guid PedidoId) : IRequest<PedidoDto>;

/// <summary>
/// Validador do command de fechamento de pedido.
/// </summary>
public class CloseOrderCommandValidator : AbstractValidator<CloseOrderCommand>
{
    public CloseOrderCommandValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O pedido é obrigatório.");
    }
}

/// <summary>
/// Handler responsável por fechar o pedido.
/// </summary>
public class CloseOrderCommandHandler : IRequestHandler<CloseOrderCommand, PedidoDto>
{
    private readonly IPedidosDbContext _context;
    private readonly IValidator<CloseOrderCommand> _validator;

    public CloseOrderCommandHandler(IPedidosDbContext context, IValidator<CloseOrderCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<PedidoDto> Handle(CloseOrderCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == request.PedidoId, cancellationToken);

        if (pedido is null)
        {
            throw new ValidationException("Pedido não encontrado.");
        }

        try
        {
            pedido.Fechar();
        }
        catch (InvalidOperationException ex)
        {
            throw new ValidationException(ex.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return PedidoDto.FromEntity(pedido);
    }
}
