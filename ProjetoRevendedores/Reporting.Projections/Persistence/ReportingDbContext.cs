using Microsoft.EntityFrameworkCore;
using Reporting.Projections.Entities;

namespace Reporting.Projections.Persistence;

/// <summary>
/// DbContext responsável pelas projeções de leitura de relatórios.
/// </summary>
public class ReportingDbContext : DbContext
{
    public ReportingDbContext(DbContextOptions<ReportingDbContext> options)
        : base(options)
    {
    }

    public DbSet<VendasPorDiaSetorView> VendasPorDiaSetorViews => Set<VendasPorDiaSetorView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReportingDbContext).Assembly);
    }
}
