using Catalogo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infrastructure.Persistence.Seed;

/// <summary>
/// Responsável por popular dados iniciais do catálogo.
/// </summary>
public static class CatalogoSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var setores = new List<Setor>
        {
            new Setor(Guid.Parse("7f2ad0df-9ea1-4d0e-8cd7-fb5d714d4b01"), "Masculino", 1, true),
            new Setor(Guid.Parse("f9fb24d3-7c2e-4b22-8a57-36262dd3cb84"), "Feminino", 2, true),
            new Setor(Guid.Parse("d214c62b-e16a-4b34-9d7a-5e2e2d98eabd"), "Infantil", 3, true),
            new Setor(Guid.Parse("2a31e6a4-3e10-4c9b-bd14-752f2824c73f"), "Acessórios", 4, true)
        };

        modelBuilder.Entity<Setor>().HasData(setores.Select(s => new
        {
            s.Id,
            s.Nome,
            s.Ordem,
            s.Ativo
        }));

        var produtos = new List<Produto>
        {
            new Produto(Guid.Parse("3f5a8c9e-b781-4af1-9bfb-7a3d67cfec21"), "Camiseta Branca", ToCentavos(49.90m), setores[0].Id, "/icons/camiseta.png", true),
            new Produto(Guid.Parse("f0b3a91c-0ce7-40dd-9aa5-b3ad59f56076"), "Calça Jeans Slim Azul", ToCentavos(139.90m), setores[0].Id, "/icons/calca.png", true),
            new Produto(Guid.Parse("c32eb407-794b-47e7-8b94-825349f4281e"), "Jaqueta Moletom Cinza", ToCentavos(189.90m), setores[0].Id, "/icons/jaqueta.png", true),

            new Produto(Guid.Parse("7b6c4622-7d1e-4ea4-80c8-861ad9381abd"), "Vestido Floral", ToCentavos(159.90m), setores[1].Id, "/icons/vestido.png", true),
            new Produto(Guid.Parse("530f8d05-0b42-4838-8f47-2e9938c1570f"), "Blusa Seda Preta", ToCentavos(89.90m), setores[1].Id, "/icons/blusa.png", true),
            new Produto(Guid.Parse("f9a01a83-2a16-4e30-b728-13475a0e5ca4"), "Saia Jeans", ToCentavos(119.90m), setores[1].Id, "/icons/saia.png", true),

            new Produto(Guid.Parse("3565cc3a-652e-4b72-919d-58c81551979c"), "Camiseta Super-Herói", ToCentavos(39.90m), setores[2].Id, "/icons/camiseta-infantil.png", true),
            new Produto(Guid.Parse("c1a890d6-79ab-4df0-9fa5-dcdaf6c7d429"), "Legging Rosa", ToCentavos(59.90m), setores[2].Id, "/icons/legging.png", true),
            new Produto(Guid.Parse("7292e04f-74b6-4823-8c38-d2ff37b18d70"), "Jaqueta Jeans Infantil", ToCentavos(109.90m), setores[2].Id, "/icons/jaqueta-infantil.png", true),

            new Produto(Guid.Parse("ad208ef7-7c0e-4f41-bc49-e18cc26be3b9"), "Boné Preto", ToCentavos(59.90m), setores[3].Id, "/icons/bone.png", true),
            new Produto(Guid.Parse("86ff2c54-3d52-4627-93d0-61aafb489a89"), "Cinto de Couro", ToCentavos(79.90m), setores[3].Id, "/icons/cinto.png", true),
            new Produto(Guid.Parse("af0fdc59-5fd5-4cd1-a3bf-2cfdf5e2ddc9"), "Mochila Casual", ToCentavos(149.90m), setores[3].Id, "/icons/mochila.png", true)
        };

        modelBuilder.Entity<Produto>().HasData(produtos.Select(p => new
        {
            p.Id,
            p.Nome,
            p.PrecoCentavos,
            p.Ativo,
            p.SetorId,
            p.IconeUrl
        }));
    }

    private static int ToCentavos(decimal valor) => (int)Math.Round(valor * 100, MidpointRounding.AwayFromZero);
}
