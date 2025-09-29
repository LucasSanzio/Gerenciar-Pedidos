using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Produtos.Models;
using Catalogo.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Produtos.Commands.CreateProduct;

/// <summary>
/// Command para criação de produto.
/// </summary>
public record CreateProductCommand(string Nome, int PrecoCentavos, Guid SetorId, string IconeUrl, bool Ativo = true) : IRequest<ProdutoDto>;

/// <summary>
/// Validador do command de criação de produto.
/// </summary>
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(200).WithMessage("O nome do produto deve ter até 200 caracteres.");

        RuleFor(x => x.PrecoCentavos)
            .GreaterThanOrEqualTo(0).WithMessage("O preço deve ser maior ou igual a zero.");

        RuleFor(x => x.SetorId)
            .NotEmpty().WithMessage("O setor é obrigatório.");

        RuleFor(x => x.IconeUrl)
            .NotEmpty().WithMessage("A URL do ícone é obrigatória.")
            .MaximumLength(500).WithMessage("A URL do ícone deve ter até 500 caracteres.");
    }
}

/// <summary>
/// Handler responsável por criar um produto no banco.
/// </summary>
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProdutoDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductCommandHandler(ICatalogoDbContext context, IValidator<CreateProductCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<ProdutoDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        // Garante que o setor existe antes de prosseguir.
        var setorExiste = await _context.Setores.AnyAsync(s => s.Id == request.SetorId, cancellationToken);
        if (!setorExiste)
        {
            throw new ValidationException("Setor informado não existe.");
        }

        var produto = new Produto(request.Nome, request.PrecoCentavos, request.SetorId, request.IconeUrl, request.Ativo);

        await _context.Produtos.AddAsync(produto, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ProdutoDto.FromEntity(produto);
    }
}
