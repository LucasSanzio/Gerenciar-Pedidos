namespace Reporting.Projections.Entities;

/// <summary>
/// Representa a projeção agregada de vendas por dia e setor.
/// </summary>
public class VendasPorDiaSetorView
{
    public DateOnly Data { get; private set; }

    public Guid SetorId { get; private set; }

    public string SetorNome { get; private set; }

    public int TotalVendidoCentavos { get; private set; }

    public int QuantidadePedidos { get; private set; }

    public int QuantidadeItens { get; private set; }

    private VendasPorDiaSetorView()
    {
        SetorNome = string.Empty;
    }

    public VendasPorDiaSetorView(DateOnly data, Guid setorId, string setorNome)
        : this()
    {
        Data = data;
        SetorId = setorId;
        SetorNome = setorNome;
    }

    public void AtualizarNome(string setorNome)
    {
        SetorNome = setorNome;
    }

    public void RegistrarVenda(int valorVendidoCentavos, int quantidadeItens, bool contarPedido)
    {
        if (valorVendidoCentavos < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valorVendidoCentavos), "O valor vendido deve ser maior ou igual a zero.");
        }

        if (quantidadeItens <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantidadeItens), "A quantidade de itens deve ser maior que zero.");
        }

        TotalVendidoCentavos += valorVendidoCentavos;
        QuantidadeItens += quantidadeItens;

        if (contarPedido)
        {
            QuantidadePedidos += 1;
        }
    }
}
