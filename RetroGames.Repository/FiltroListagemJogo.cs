namespace RetroGames.Repository;

/// <summary>
/// Filtros e paginação da listagem de jogos. Cada filtro só vale quando é informado
/// (valor diferente de <c>null</c>).
/// </summary>
public class FiltroListagemJogo
{
    /// <summary>Parte do nome do jogo (busca textual). <c>null</c> = sem filtro.</summary>
    public string? Busca { get; set; }

    /// <summary>Id da plataforma. <c>null</c> = todas as plataformas.</summary>
    public int? IdPlataforma { get; set; }

    /// <summary>
    /// Restringe a listagem a estes ids de jogo (o Service usa para o filtro por gênero).
    /// <c>null</c> = sem filtro. Lista vazia = nenhum jogo atende ao filtro.
    /// </summary>
    public IReadOnlyCollection<int>? Ids { get; set; }

    /// <summary>Se verdadeiro, traz também os jogos inativos (somente Administrador, RN04).</summary>
    public bool IncluirInativos { get; set; }

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; } = 1;

    /// <summary>Quantidade de jogos por página.</summary>
    public int TamanhoPagina { get; set; } = 10;
}
