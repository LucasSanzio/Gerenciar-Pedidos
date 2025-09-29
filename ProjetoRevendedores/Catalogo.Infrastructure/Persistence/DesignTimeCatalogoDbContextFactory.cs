using Catalogo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Catalogo.Infrastructure.Persistence;

/// <summary>
/// Factory utilizada pelo EF Core durante as migrations.
/// </summary>
public class DesignTimeCatalogoDbContextFactory : IDesignTimeDbContextFactory<CatalogoDbContext>
{
    public CatalogoDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CatalogoDbContext>();
        var connectionString = configuration.GetConnectionString("Catalogo")
            ?? "Server=(localdb)\\mssqllocaldb;Database=ProjetoRevendedoresCatalogo;Trusted_Connection=True;MultipleActiveResultSets=true";

        optionsBuilder.UseSqlServer(connectionString);

        return new CatalogoDbContext(optionsBuilder.Options);
    }
}
