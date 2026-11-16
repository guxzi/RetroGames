using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

/// <summary>
/// Regras de negócio das plataformas. Não tem SQL nem nada de HTTP: confere as regras
/// consultando o Repository (repositorio.Plataforma) e, se tudo estiver certo, manda gravar.
/// </summary>
public class PlataformaService : IPlataformaService
{
    private readonly IRepositorySet _repositorio;

    public PlataformaService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public Task<List<Plataforma>> ListarAsync()
    {
        return _repositorio.Plataforma.ListarAsync();
    }

    public async Task<Plataforma> BuscarPorIdAsync(int id)
    {
        var plataforma = await _repositorio.Plataforma.BuscarPorIdAsync(id);
        if (plataforma is null)
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaNaoEncontrada);
        }
        return plataforma;
    }

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: o nome não pode estar em uso.
        if (await _repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome))
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
        }

        try
        {
            plataforma.Id = await _repositorio.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            // Duas gravações ao mesmo tempo passaram pela conferência acima e o UNIQUE
            // do banco recusou a segunda: é o mesmo erro de negócio (RN01).
            throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
        }

        return plataforma;
    }

    public async Task<Plataforma> AlterarAsync(Plataforma plataforma)
    {
        // Lança PlataformaNaoEncontrada se o id não existir.
        await BuscarPorIdAsync(plataforma.Id);

        plataforma.Nome = plataforma.Nome.Trim();

        // RN01, ignorando a própria plataforma: manter o próprio nome não é duplicidade.
        if (await _repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome, plataforma.Id))
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
        }

        try
        {
            await _repositorio.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaNomeDuplicado);
        }

        return plataforma;
    }

    public async Task ExcluirAsync(int id)
    {
        await BuscarPorIdAsync(id);

        // RN02: plataforma com jogos não pode ser excluída.
        if (await _repositorio.Plataforma.PossuiJogosAsync(id))
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaPossuiJogos);
        }

        await _repositorio.Plataforma.ExcluirAsync(id);
    }
}
