using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

/// <summary>
/// Service dos gêneros. Não tem regra, mas existe porque os controllers só falam
/// com Services, nunca direto com o Repository.
/// </summary>
public class GeneroService : IGeneroService
{
    private readonly IRepositorySet _repositorio;

    public GeneroService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<List<Genero>> ListarAsync()
    {
        return _repositorio.Genero.ListarAsync();
    }
}
