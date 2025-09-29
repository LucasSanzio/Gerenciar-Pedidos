using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Produtos.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Produtos.Commands.ActivateProduct;

/// <summary>
/// Command para ativar um produto.
/// </summary>
public record ActivateProductCommand(Guid Id) : IRequest<ProdutoDto>;

public class ActivateProductCommandValidator : AbstractValidator<ActivateProductCommand>
{
    public ActivateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, ProdutoDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<ActivateProductCommand> _validator;

    public ActivateProductCommandHandler(ICatalogoDbContext context, IValidator<ActivateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ProdutoDto> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (produto is null)
        {
            throw new ValidationException("Produto não encontrado.");
        }

        produto.Ativar();
        await _context.SaveChangesAsync(cancellationToken);

        return ProdutoDto.FromEntity(produto);
    }
}
