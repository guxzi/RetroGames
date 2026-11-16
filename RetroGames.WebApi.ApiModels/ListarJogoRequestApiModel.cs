namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// Parâmetros da listagem de jogos, lidos da query string:
/// GET /api/Jogo?busca=zelda&amp;idPlataforma=2&amp;idGenero=1&amp;pagina=1&amp;tamanhoPagina=12
/// </summary>
public class ListarJogoRequestApiModel
{
    /// <summary>Parte do nome do jogo. Vazio = sem filtro.</summary>
    public string? Busca { get; set; }

    /// <summary>Id da plataforma. Vazio = todas.</summary>
    public int? IdPlataforma { get; set; }

    /// <summary>Id do gênero. Vazio = todos.</summary>
    public int? IdGenero { get; set; }

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; } = 1;

    /// <summary>Quantidade de jogos por página.</summary>
    public int TamanhoPagina { get; set; } = 12;
}
