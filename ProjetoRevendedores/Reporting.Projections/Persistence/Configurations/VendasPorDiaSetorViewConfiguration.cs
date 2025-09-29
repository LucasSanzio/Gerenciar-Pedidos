using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reporting.Projections.Entities;

namespace Reporting.Projections.Persistence.Configurations;

/// <summary>
/// Configuração da projeção de vendas por dia e setor.
/// </summary>
public class VendasPorDiaSetorViewConfiguration : IEntityTypeConfiguration<VendasPorDiaSetorView>
{
    public void Configure(EntityTypeBuilder<VendasPorDiaSetorView> builder)
    {
        builder.ToTable("VendasPorDiaSetorView");

        builder.HasKey(v => new { v.Data, v.SetorId });

        builder.Property(v => v.Data)
            .HasColumnType("date")
            .HasConversion(
                value => value.ToDateTime(TimeOnly.MinValue),
                value => DateOnly.FromDateTime(value));

        builder.Property(v => v.SetorNome)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.TotalVendidoCentavos)
            .IsRequired();

        builder.Property(v => v.QuantidadePedidos)
            .IsRequired();

        builder.Property(v => v.QuantidadeItens)
            .IsRequired();
    }
}
