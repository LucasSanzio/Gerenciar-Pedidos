using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Produtos.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Produtos.Commands.DeactivateProduct;

/// <summary>
/// Command para desativar um produto.
/// </summary>
public record DeactivateProductCommand(Guid Id) : IRequest<ProdutoDto>;

public class DeactivateProductCommandValidator : AbstractValidator<DeactivateProductCommand>
{
    public DeactivateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand, ProdutoDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<DeactivateProductCommand> _validator;

    public DeactivateProductCommandHandler(ICatalogoDbContext context, IValidator<DeactivateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ProdutoDto> Handle(DeactivateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (produto is null)
        {
            throw new ValidationException("Produto não encontrado.");
        }

        produto.Desativar();
        await _context.SaveChangesAsync(cancellationToken);

        return ProdutoDto.FromEntity(produto);
    }
}
