using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

/// <summary>
/// Service das condições de jogo. Não tem regra, mas existe porque os controllers só
/// falam com Services, nunca direto com o Repository.
/// </summary>
public class CondicaoJogoService : ICondicaoJogoService
{
    private readonly IRepositorySet _repositorio;

    public CondicaoJogoService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<List<CondicaoJogo>> ListarAsync()
    {
        return _repositorio.CondicaoJogo.ListarAsync();
    }
}
