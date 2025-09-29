using System;
using Catalogo.Domain.Entities;
using Xunit;

namespace Catalogo.Domain.Tests;

public class ProdutoTests
{
    [Fact]
    public void Atualizar_DeveLancarErro_QuandoPrecoForNegativo()
    {
        var setorId = Guid.NewGuid();
        var produto = new Produto("Camiseta", 1000, setorId, "https://exemplo.com/camiseta.svg");

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            produto.Atualizar("Camiseta", -1, "https://exemplo.com/camiseta.svg", setorId, true));

        Assert.Contains("O preço deve ser maior ou igual a zero.", exception.Message);
    }

    [Fact]
    public void Construtor_DeveDefinirPropriedadesValidas()
    {
        var setorId = Guid.NewGuid();

        var produto = new Produto("Calça Jeans", 2599, setorId, "https://exemplo.com/calca.svg", true);

        Assert.Equal("Calça Jeans", produto.Nome);
        Assert.Equal(2599, produto.PrecoCentavos);
        Assert.Equal(setorId, produto.SetorId);
        Assert.True(produto.Ativo);
        Assert.Equal("https://exemplo.com/calca.svg", produto.iconeUrl);
    }
}
