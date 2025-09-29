namespace Catalogo.Domain.Entities;

/// <summary>
/// Representa um produto do catálogo.
/// </summary>
public class Produto
{
    /// <summary>
    /// Identificador único do produto.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Nome do produto.
    /// </summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Preço do produto em centavos.
    /// </summary>
    public int PrecoCentavos { get; private set; }

    /// <summary>
    /// Indica se o produto está ativo para venda.
    /// </summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Identificador do setor ao qual o produto pertence.
    /// </summary>
    public Guid SetorId { get; private set; }

    /// <summary>
    /// URL do ícone exibido no catálogo.
    /// </summary>
    public string IconeUrl { get; private set; }

    /// <summary>
    /// Navegação para o setor.
    /// </summary>
    public Setor? Setor { get; private set; }

    /// <summary>
    /// Construtor requerido pelo EF Core.
    /// </summary>
    private Produto()
    {
        Nome = string.Empty;
        IconeUrl = string.Empty;
    }

    public Produto(string nome, int precoCentavos, Guid setorId, string iconeUrl, bool ativo = true)
        : this()
    {
        Id = Guid.NewGuid();
        Atualizar(nome, precoCentavos, iconeUrl, setorId, ativo);
    }

    internal Produto(Guid id, string nome, int precoCentavos, Guid setorId, string iconeUrl, bool ativo) : this()
    {
        Id = id;
        Atualizar(nome, precoCentavos, iconeUrl, setorId, ativo);
    }

    /// <summary>
    /// Atualiza propriedades principais do produto.
    /// </summary>
    public void Atualizar(string nome, int precoCentavos, string iconeUrl, Guid setorId, bool ativo)
    {
        if (precoCentavos < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(precoCentavos), "O preço deve ser maior ou igual a zero.");
        }

        Nome = nome.Trim();
        PrecoCentavos = precoCentavos;
        IconeUrl = iconeUrl.Trim();
        SetorId = setorId;
        Ativo = ativo;
    }

    /// <summary>
    /// Ativa o produto.
    /// </summary>
    public void Ativar() => Ativo = true;

    /// <summary>
    /// Desativa o produto.
    /// </summary>
    public void Desativar() => Ativo = false;
}
