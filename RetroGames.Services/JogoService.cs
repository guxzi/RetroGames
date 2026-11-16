using System.Transactions;
using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services;

/// <summary>
/// Regras de negócio dos jogos. Junta, com LINQ, o que o Repository devolve separado
/// (jogos, plataformas, condições, gêneros e vínculos) e grava jogo e gêneros numa
/// transação. Não tem SQL nem nada de HTTP.
/// </summary>
public class JogoService : IJogoService
{
    private readonly IRepositorySet _repositorio;

    public JogoService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<(List<JogoDetalheServiceModel> Jogos, int Total)> ListarAsync(
        FiltroListagemJogo filtro, int? idGenero)
    {
        // RN04: a consulta pública só mostra jogos ativos. Jogos inativos para o
        // administrador ficam para a aula 6, com a autenticação.
        filtro.IncluirInativos = false;

        if (idGenero is not null)
        {
            // Filtro por gênero: os vínculos estão em outra tabela, então o Service
            // seleciona com LINQ os ids dos jogos do gênero e entrega ao Repository.
            // A contagem e a paginação continuam no SQL.
            var vinculos = await _repositorio.JogoGenero.ListarTodosAsync();
            filtro.Ids = vinculos
                .Where(v => v.IdGenero == idGenero)
                .Select(v => v.IdJogo)
                .Distinct()
                .ToList();
        }

        var (jogos, total) = await _repositorio.Jogo.ListarAsync(filtro);
        var detalhes = await MontarDetalhesAsync(jogos);
        return (detalhes, total);
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await BuscarJogoExistenteAsync(id);
        var detalhes = await MontarDetalhesAsync(new List<Jogo> { jogo });
        return detalhes[0];
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros)
    {
        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true;
        jogo.DataCadastro = DateTime.UtcNow;

        var generos = idsGeneros.Distinct().ToList();
        await ConferirReferenciasAsync(jogo, generos);

        // Jogo e vínculos: ou grava tudo, ou nada.
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await _repositorio.Jogo.InserirAsync(jogo);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros)
    {
        await BuscarJogoExistenteAsync(jogo.Id);

        jogo.Nome = jogo.Nome.Trim();

        var generos = idsGeneros.Distinct().ToList();
        await ConferirReferenciasAsync(jogo, generos);

        // Jogo, remoção dos vínculos antigos e gravação dos novos: ou tudo, ou nada.
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repositorio.Jogo.AlterarAsync(jogo);
            await _repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, generos);
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task DesativarAsync(int id)
    {
        await BuscarJogoExistenteAsync(id);

        // RN03: exclusão lógica. O registro e os vínculos são preservados.
        await _repositorio.Jogo.DesativarAsync(id);
    }

    /// <summary>O jogo do id informado. Se não existir, lança JogoNaoEncontrado.</summary>
    private async Task<Jogo> BuscarJogoExistenteAsync(int id)
    {
        var jogo = await _repositorio.Jogo.BuscarPorIdAsync(id);
        if (jogo is null)
        {
            throw new BusinessRuleException(BusinessErrors.JogoNaoEncontrado);
        }
        return jogo;
    }

    /// <summary>
    /// RN05: o jogo precisa referenciar plataforma, condição e ao menos um gênero que existam.
    /// </summary>
    private async Task ConferirReferenciasAsync(Jogo jogo, IReadOnlyCollection<int> idsGeneros)
    {
        if (await _repositorio.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma) is null)
        {
            throw new BusinessRuleException(BusinessErrors.PlataformaNaoEncontrada);
        }

        var condicoes = await _repositorio.CondicaoJogo.ListarAsync();
        if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
        {
            throw new BusinessRuleException(BusinessErrors.CondicaoNaoEncontrada);
        }

        var idsExistentes = (await _repositorio.Genero.ListarAsync()).Select(g => g.Id).ToHashSet();
        if (idsGeneros.Count == 0 || idsGeneros.Any(id => !idsExistentes.Contains(id)))
        {
            throw new BusinessRuleException(BusinessErrors.GeneroNaoEncontrado);
        }
    }

    /// <summary>
    /// Monta o detalhe de uma lista de jogos com LINQ. Faz sempre as mesmas quatro
    /// consultas (plataformas, condições, gêneros e vínculos dos jogos da lista),
    /// qualquer que seja o tamanho da lista: nunca uma consulta por jogo.
    /// </summary>
    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(List<Jogo> jogos)
    {
        if (jogos.Count == 0)
        {
            return new List<JogoDetalheServiceModel>();
        }

        var idsJogos = jogos.Select(j => j.Id).ToList();

        var plataformas = (await _repositorio.Plataforma.ListarAsync()).ToDictionary(p => p.Id);
        var condicoes = (await _repositorio.CondicaoJogo.ListarAsync()).ToDictionary(c => c.Id);
        var generos = (await _repositorio.Genero.ListarAsync()).ToDictionary(g => g.Id);
        var vinculos = await _repositorio.JogoGenero.ListarPorJogosAsync(idsJogos);

        // Para cada jogo, a lista dos seus gêneros (já ordenada por nome).
        var generosPorJogo = vinculos
            .Where(v => generos.ContainsKey(v.IdGenero))
            .GroupBy(v => v.IdJogo)
            .ToDictionary(
                grupo => grupo.Key,
                grupo => grupo.Select(v => generos[v.IdGenero]).OrderBy(g => g.Nome).ToList());

        return jogos
            .Select(jogo => new JogoDetalheServiceModel
            {
                Jogo = jogo,
                NomePlataforma = plataformas.TryGetValue(jogo.IdPlataforma, out var plataforma)
                    ? plataforma.Nome
                    : string.Empty,
                DescricaoCondicao = condicoes.TryGetValue(jogo.IdCondicaoJogo, out var condicao)
                    ? condicao.Descricao
                    : string.Empty,
                Generos = generosPorJogo.TryGetValue(jogo.Id, out var generosDoJogo)
                    ? generosDoJogo
                    : new List<Genero>(),
            })
            .ToList();
    }
}
