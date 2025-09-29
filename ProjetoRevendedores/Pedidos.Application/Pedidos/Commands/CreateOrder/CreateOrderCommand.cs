using FluentValidation;
using MediatR;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Application.Pedidos.Models;
using Pedidos.Domain.Entities;

namespace Pedidos.Application.Pedidos.Commands.CreateOrder;

/// <summary>
/// Command responsável por criar um novo pedido.
/// </summary>
public record CreateOrderCommand(string? Observacao) : IRequest<PedidoDto>;

/// <summary>
/// Validador do command de criação de pedido.
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Observacao)
            .MaximumLength(500).WithMessage("A observação deve ter no máximo 500 caracteres.");
    }
}

/// <summary>
/// Handler responsável por criar o pedido no banco.
/// </summary>
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, PedidoDto>
{
    private readonly IPedidosDbContext _context;
    private readonly IValidator<CreateOrderCommand> _validator;

    public CreateOrderCommandHandler(IPedidosDbContext context, IValidator<CreateOrderCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<PedidoDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var pedido = new Pedido(request.Observacao);

        await _context.Pedidos.AddAsync(pedido, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return PedidoDto.FromEntity(pedido);
    }
}
