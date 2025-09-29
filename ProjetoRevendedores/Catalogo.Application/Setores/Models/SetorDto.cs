using Catalogo.Domain.Entities;

namespace Catalogo.Application.Setores.Models;

/// <summary>
/// DTO de retorno para setor.
/// </summary>
public record SetorDto(
    Guid Id,
    string Nome,
    int Ordem,
    bool Ativo)
{
    public static SetorDto FromEntity(Setor setor) =>
        new(setor.Id, setor.Nome, setor.Ordem, setor.Ativo);
}
