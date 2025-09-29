using Catalogo.Application.Common.Interfaces;
using Catalogo.Domain.Entities;
using Catalogo.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Persistence;

/// <summary>
/// DbContext principal do módulo de catálogo.
/// </summary>
public class CatalogoDbContext : DbContext, ICatalogoDbContext
{
    public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options) : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Setor> Setores => Set<Setor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDbContext).Assembly);
        CatalogoSeed.Seed(modelBuilder);
    }
}
