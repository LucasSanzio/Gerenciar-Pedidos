using Catalogo.Domain.Entities;

namespace Catalogo.Application.Produtos.Models;

/// <summary>
/// Representa os dados retornados pela API sobre um produto.
/// </summary>
public record ProdutoDto(
    Guid Id,
    string Nome,
    int PrecoCentavos,
    bool Ativo,
    Guid SetorId,
    string IconeUrl)
{
    public static ProdutoDto FromEntity(Produto produto) =>
        new(produto.Id, produto.Nome, produto.PrecoCentavos, produto.Ativo, produto.SetorId, produto.IconeUrl);
}
