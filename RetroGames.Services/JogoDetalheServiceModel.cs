using RetroGames.Model.Model;

namespace RetroGames.Services;

/// <summary>
/// O jogo com os dados que vêm de outras tabelas: nome da plataforma, descrição da
/// condição e gêneros. É montado no JogoService com LINQ, porque o Repository só
/// devolve dados de uma tabela por consulta.
/// </summary>
public class JogoDetalheServiceModel
{
    /// <summary>O jogo, como veio de tb_jogo.</summary>
    public Jogo Jogo { get; set; } = new();

    /// <summary>Nome da plataforma do jogo (tb_plataforma).</summary>
    public string NomePlataforma { get; set; } = string.Empty;

    /// <summary>Descrição da condição do jogo (tb_condicao_jogo).</summary>
    public string DescricaoCondicao { get; set; } = string.Empty;

    /// <summary>Gêneros do jogo (tb_genero, via tb_jogo_genero), ordenados por nome.</summary>
    public List<Genero> Generos { get; set; } = new();
}
