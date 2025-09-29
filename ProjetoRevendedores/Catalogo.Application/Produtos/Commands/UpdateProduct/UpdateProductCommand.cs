using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Produtos.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Produtos.Commands.UpdateProduct;

/// <summary>
/// Command para atualização de produto.
/// </summary>
public record UpdateProductCommand(Guid Id, string Nome, int PrecoCentavos, Guid SetorId, string IconeUrl, bool Ativo) : IRequest<ProdutoDto>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200);
        RuleFor(x => x.PrecoCentavos)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.SetorId)
            .NotEmpty();
        RuleFor(x => x.IconeUrl)
            .NotEmpty()
            .MaximumLength(500);
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProdutoDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<UpdateProductCommand> _validator;

    public UpdateProductCommandHandler(ICatalogoDbContext context, IValidator<UpdateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ProdutoDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (produto is null)
        {
            throw new ValidationException("Produto não encontrado.");
        }

        var setorExiste = await _context.Setores.AnyAsync(s => s.Id == request.SetorId, cancellationToken);
        if (!setorExiste)
        {
            throw new ValidationException("Setor informado não existe.");
        }

        produto.Atualizar(request.Nome, request.PrecoCentavos, request.IconeUrl, request.SetorId, request.Ativo);

        await _context.SaveChangesAsync(cancellationToken);

        return ProdutoDto.FromEntity(produto);
    }
}
