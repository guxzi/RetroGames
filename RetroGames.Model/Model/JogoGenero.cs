namespace RetroGames.Model.Model;

/// <summary>Associação N:N entre jogo e gênero (tabela tb_jogo_genero).</summary>
public class JogoGenero
{
    /// <summary>co_jogo (FK para tb_jogo, parte da PK composta)</summary>
    public int IdJogo { get; set; }

    /// <summary>co_genero (FK para tb_genero, parte da PK composta)</summary>
    public int IdGenero { get; set; }
}
