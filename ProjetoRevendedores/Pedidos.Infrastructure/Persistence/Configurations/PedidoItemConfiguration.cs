using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pedidos.Domain.Entities;

namespace Pedidos.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configurações para a entidade PedidoItem.
/// </summary>
public class PedidoItemConfiguration : IEntityTypeConfiguration<PedidoItem>
{
    public void Configure(EntityTypeBuilder<PedidoItem> builder)
    {
        builder.ToTable("PedidoItens");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PedidoId)
            .IsRequired();

        builder.Property(p => p.ProdutoId)
            .IsRequired();

        builder.Property(p => p.NomeProduto)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(p => p.PrecoCentavos)
            .IsRequired();
    }
}
