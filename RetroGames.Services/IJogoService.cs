using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

/// <summary>Regras de negócio dos jogos (RN03, RN04 e RN05).</summary>
public interface IJogoService
{
    /// <summary>
    /// A página pedida, com o detalhe de cada jogo, e o total de jogos encontrados.
    /// <paramref name="idGenero"/> filtra por gênero (resolvido com LINQ no Service).
    /// </summary>
    Task<(List<JogoDetalheServiceModel> Jogos, int Total)> ListarAsync(FiltroListagemJogo filtro, int? idGenero);

    /// <summary>O detalhe do jogo. Se não existir: JogoNaoEncontrado.</summary>
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);

    /// <summary>Grava um novo jogo, ativo, com os seus gêneros, e devolve o detalhe dele.</summary>
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros);

    /// <summary>Altera o jogo e troca os seus gêneros, e devolve o detalhe dele.</summary>
    Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros);

    /// <summary>Marca o jogo como inativo. Jogo nunca é apagado (RN03).</summary>
    Task DesativarAsync(int id);
}
