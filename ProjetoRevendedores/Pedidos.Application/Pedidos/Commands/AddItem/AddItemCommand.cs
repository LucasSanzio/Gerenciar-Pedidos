using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pedidos.Application.Common.Interfaces;
using Pedidos.Application.Pedidos.Models;

namespace Pedidos.Application.Pedidos.Commands.AddItem;

/// <summary>
/// Command responsável por adicionar um item ao pedido.
/// </summary>
public record AddItemCommand(Guid PedidoId, Guid ProdutoId, Guid SetorId, string SetorNome, string NomeProduto, int PrecoCentavos) : IRequest<PedidoDto>;

/// <summary>
/// Validador do command de adição de item.
/// </summary>
public class AddItemCommandValidator : AbstractValidator<AddItemCommand>
{
    public AddItemCommandValidator()
    {
        RuleFor(x => x.PedidoId)
            .NotEmpty().WithMessage("O pedido é obrigatório.");

        RuleFor(x => x.ProdutoId)
            .NotEmpty().WithMessage("O produto é obrigatório.");

        RuleFor(x => x.SetorId)
            .NotEmpty().WithMessage("O setor é obrigatório.");

        RuleFor(x => x.SetorNome)
            .NotEmpty().WithMessage("O nome do setor é obrigatório.")
            .MaximumLength(200).WithMessage("O nome do setor deve ter no máximo 200 caracteres.");

        RuleFor(x => x.NomeProduto)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200).WithMessage("O nome do produto deve ter no máximo 200 caracteres.");

        RuleFor(x => x.PrecoCentavos)
            .GreaterThanOrEqualTo(0).WithMessage("O preço deve ser maior ou igual a zero.");
    }
}

/// <summary>
/// Handler responsável por persistir o item no pedido.
/// </summary>
public class AddItemCommandHandler : IRequestHandler<AddItemCommand, PedidoDto>
{
    private readonly IPedidosDbContext _context;
    private readonly IValidator<AddItemCommand> _validator;

    public AddItemCommandHandler(IPedidosDbContext context, IValidator<AddItemCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<PedidoDto> Handle(AddItemCommand request, CancellationToken cancellationToken)
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
            pedido.AdicionarItem(request.ProdutoId, request.SetorId, request.SetorNome, request.NomeProduto, request.PrecoCentavos);
        }
        catch (InvalidOperationException ex)
        {
            throw new ValidationException(ex.Message);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return PedidoDto.FromEntity(pedido);
    }
}
