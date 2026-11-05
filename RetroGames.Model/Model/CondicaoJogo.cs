namespace RetroGames.Model.Model;

/// <summary>Condição de conservação do jogo (tabela tb_condicao_jogo).</summary>
public class CondicaoJogo
{
    /// <summary>co_condicao_jogo (chave fixa, sem AUTO_INCREMENT)</summary>
    public int Id { get; set; }

    /// <summary>ds_condicao_jogo</summary>
    public string Descricao { get; set; } = string.Empty;
}
