using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Setores.Models;
using Catalogo.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Setores.Commands.CreateSetor;

/// <summary>
/// Command para criação de setor.
/// </summary>
public record CreateSetorCommand(string Nome, int Ordem, bool Ativo = true) : IRequest<SetorDto>;

public class CreateSetorCommandValidator : AbstractValidator<CreateSetorCommand>
{
    public CreateSetorCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome do setor é obrigatório.")
            .MaximumLength(150).WithMessage("O nome do setor deve ter até 150 caracteres.");

        RuleFor(x => x.Ordem)
            .GreaterThanOrEqualTo(0).WithMessage("A ordem deve ser maior ou igual a zero.");
    }
}

public class CreateSetorCommandHandler : IRequestHandler<CreateSetorCommand, SetorDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<CreateSetorCommand> _validator;

    public CreateSetorCommandHandler(ICatalogoDbContext context, IValidator<CreateSetorCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<SetorDto> Handle(CreateSetorCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var nomeNormalizado = request.Nome.Trim().ToLowerInvariant();
        var setorExiste = await _context.Setores
            .AnyAsync(s => s.Nome.ToLower() == nomeNormalizado, cancellationToken);
        if (setorExiste)
        {
            throw new ValidationException("Já existe um setor com este nome.");
        }

        var setor = new Setor(request.Nome, request.Ordem, request.Ativo);
        await _context.Setores.AddAsync(setor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return SetorDto.FromEntity(setor);
    }
}
