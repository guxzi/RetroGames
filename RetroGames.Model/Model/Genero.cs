namespace RetroGames.Model.Model;

/// <summary>Gênero do jogo (tabela tb_genero).</summary>
public class Genero
{
    /// <summary>co_seq_genero</summary>
    public int Id { get; set; }

    /// <summary>no_genero</summary>
    public string Nome { get; set; } = string.Empty;
}
