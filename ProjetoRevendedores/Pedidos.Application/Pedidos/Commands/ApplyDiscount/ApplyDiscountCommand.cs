using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Application.Pedidos.Models;

namespace Pedidos.Application.Pedidos.Commands.ApplyDiscount;

/// <summary>
/// Command responsável por aplicar desconto a um pedido.
/// </summary>
public record ApplyDiscountCommand(Guid PedidoId, int DescontoCentavos) : IRequest<PedidoDto>;

/// <summary>
/// Validador do command de desconto.
/// </summary>
public class ApplyDiscountCommandValidator : AbstractValidator<ApplyDiscountCommand>
{
    public ApplyDiscountCommandValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O pedido é obrigatório.");

        RuleFor(x => x.DescontoCentavos)
            .GreaterThanOrEqualTo(0).WithMessage("O desconto deve ser maior ou igual a zero.");
    }
}

/// <summary>
/// Handler responsável por aplicar o desconto no pedido.
/// </summary>
public class ApplyDiscountCommandHandler : IRequestHandler<ApplyDiscountCommand, PedidoDto>
{
    private readonly IPedidosDbContext _context;
    private readonly IValidator<ApplyDiscountCommand> _validator;

    public ApplyDiscountCommandHandler(IPedidosDbContext context, IValidator<ApplyDiscountCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<PedidoDto> Handle(ApplyDiscountCommand request, CancellationToken cancellationToken)
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
            pedido.AplicarDesconto(request.DescontoCentavos);
        }
        catch (InvalidOperationException ex)
        {
            throw new ValidationException(ex.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return PedidoDto.FromEntity(pedido);
    }
}
