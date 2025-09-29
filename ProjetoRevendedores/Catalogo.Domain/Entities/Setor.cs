using System.Collections.Generic;

namespace Catalogo.Domain.Entities;

/// <summary>
/// Representa um setor do catálogo (ex: Masculino, Feminino).
/// </summary>
public class Setor
{
    /// <summary>
    /// Identificador único do setor.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Nome do setor.
    /// </summary>
    public string Nome { get; private set; }

    /// <summary>
    /// Ordem de exibição do setor.
    /// </summary>
    public int Ordem { get; private set; }

    /// <summary>
    /// Indica se o setor está ativo.
    /// </summary>
    public bool Ativo { get; private set; }

    /// <summary>
    /// Lista de produtos associados ao setor.
    /// </summary>
    public ICollection<Produto> Produtos { get; private set; }

    /// <summary>
    /// Construtor requerido pelo EF Core.
    /// </summary>
    private Setor()
    {
        Produtos = new List<Produto>();
        Nome = string.Empty;
    }

    public Setor(string nome, int ordem, bool ativo = true)
        : this()
    {
        Id = Guid.NewGuid();
        Atualizar(nome, ordem, ativo);
    }

    internal Setor(Guid id, string nome, int ordem, bool ativo) : this()
    {
        Id = id;
        Atualizar(nome, ordem, ativo);
    }

    /// <summary>
    /// Atualiza informações básicas do setor.
    /// </summary>
    public void Atualizar(string nome, int ordem, bool ativo)
    {
        Nome = nome.Trim();
        Ordem = ordem;
        Ativo = ativo;
    }

    /// <summary>
    /// Ativa o setor.
    /// </summary>
    public void Ativar() => Ativo = true;

    /// <summary>
    /// Desativa o setor.
    /// </summary>
    public void Desativar() => Ativo = false;
}
