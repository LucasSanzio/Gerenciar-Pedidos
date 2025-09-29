using Catalogo.Application.Common.Interfaces;
using Catalogo.Application.Setores.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Setores.Commands.UpdateSetor;

/// <summary>
/// Command para atualização de um setor existente.
/// </summary>
public record UpdateSetorCommand(Guid Id, string Nome, int Ordem, bool Ativo) : IRequest<SetorDto>;

public class UpdateSetorCommandValidator : AbstractValidator<UpdateSetorCommand>
{
    public UpdateSetorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(150);
        RuleFor(x => x.Ordem)
            .GreaterThanOrEqualTo(0);
    }
}

public class UpdateSetorCommandHandler : IRequestHandler<UpdateSetorCommand, SetorDto>
{
    private readonly ICatalogoDbContext _context;
    private readonly IValidator<UpdateSetorCommand> _validator;

    public UpdateSetorCommandHandler(ICatalogoDbContext context, IValidator<UpdateSetorCommand> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<SetorDto> Handle(UpdateSetorCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var setor = await _context.Setores.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
        if (setor is null)
        {
            throw new ValidationException("Setor não encontrado.");
        }

        var nomeNormalizado = request.Nome.Trim().ToLowerInvariant();
        var existeOutroSetor = await _context.Setores
            .AnyAsync(s => s.Id != request.Id && s.Nome.ToLower() == nomeNormalizado, cancellationToken);
        if (existeOutroSetor)
        {
            throw new ValidationException("Já existe outro setor com este nome.");
        }

        setor.Atualizar(request.Nome, request.Ordem, request.Ativo);
        await _context.SaveChangesAsync(cancellationToken);

        return SetorDto.FromEntity(setor);
    }
}
