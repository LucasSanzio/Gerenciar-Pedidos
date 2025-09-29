using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogo.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração da entidade Produto.
/// </summary>
public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.IconeUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.PrecoCentavos)
            .IsRequired();

        builder.Property(p => p.Ativo)
            .IsRequired();

        builder.Property(p => p.SetorId)
            .IsRequired();

        builder.HasOne(p => p.Setor)
            .WithMany(s => s.Produtos)
            .HasForeignKey(p => p.SetorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
