namespace RetroGames.Model.Model;

/// <summary>Jogo à venda (tabela tb_jogo).</summary>
public class Jogo
{
    /// <summary>co_seq_jogo</summary>
    public int Id { get; set; }

    /// <summary>no_jogo</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>ds_jogo (opcional: única coluna que aceita NULL)</summary>
    public string? Descricao { get; set; }

    /// <summary>nu_ano_lancamento</summary>
    public int AnoLancamento { get; set; }

    /// <summary>vl_preco</summary>
    public decimal Preco { get; set; }

    /// <summary>qt_estoque</summary>
    public int Estoque { get; set; }

    /// <summary>st_ativo</summary>
    public bool Ativo { get; set; }

    /// <summary>dt_cadastro (UTC)</summary>
    public DateTime DataCadastro { get; set; }

    /// <summary>co_plataforma (FK para tb_plataforma)</summary>
    public int IdPlataforma { get; set; }

    /// <summary>co_condicao_jogo (FK para tb_condicao_jogo)</summary>
    public int IdCondicaoJogo { get; set; }
}
