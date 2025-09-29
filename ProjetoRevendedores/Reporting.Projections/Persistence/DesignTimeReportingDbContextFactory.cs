using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Reporting.Projections.Persistence;

/// <summary>
/// Factory utilizada em tempo de design para geração de migrations.
/// </summary>
public class DesignTimeReportingDbContextFactory : IDesignTimeDbContextFactory<ReportingDbContext>
{
    public ReportingDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReportingDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresReporting;Trusted_Connection=True;MultipleActiveResultSets=true");

        return new ReportingDbContext(optionsBuilder.Options);
    }
}
