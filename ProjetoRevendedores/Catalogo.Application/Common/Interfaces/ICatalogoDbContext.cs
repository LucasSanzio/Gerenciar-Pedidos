using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Application.Common.Interfaces;

/// <summary>
/// Define o contrato mínimo utilizado pelas camadas de aplicação para acessar o banco.
/// </summary>
public interface ICatalogoDbContext
{
    DbSet<Produto> Produtos { get; }
    DbSet<Setor> Setores { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
