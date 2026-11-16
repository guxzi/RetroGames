using RetroGames.Model.Model;

namespace RetroGames.Services;

/// <summary>Acesso às condições de jogo (somente leitura, sem regra de negócio).</summary>
public interface ICondicaoJogoService
{
    /// <summary>Todas as condições de jogo, na ordem das chaves fixas.</summary>
    Task<List<CondicaoJogo>> ListarAsync();
}
