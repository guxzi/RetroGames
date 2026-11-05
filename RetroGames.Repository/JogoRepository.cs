using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

/// <summary>Todo o SQL da tabela tb_jogo. Nenhuma consulta usa JOIN.</summary>
public class JogoRepository : RepositoryBase, IJogoRepository
{
    private const string Colunas =
        "co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
        "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo";

    public JogoRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<(List<Jogo> Jogos, int Total)> ListarAsync(FiltroListagemJogo filtro)
    {
        await using var conexao = await AbrirConexaoAsync();

        // 1) Total de jogos que atendem ao filtro (sem paginação).
        int total;
        await using (var comandoTotal = CriarComando(conexao, string.Empty))
        {
            var where = MontarFiltro(comandoTotal, filtro);
            comandoTotal.CommandText = $"SELECT COUNT(*) FROM tb_jogo{where}";
            total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());
        }

        // 2) Os jogos da página, com o MESMO filtro.
        await using var comando = CriarComando(conexao, string.Empty);
        var filtroPagina = MontarFiltro(comando, filtro);
        comando.CommandText =
            $"SELECT {Colunas} FROM tb_jogo{filtroPagina} " +
            "ORDER BY no_jogo, co_seq_jogo " +
            "LIMIT @tamanhoPagina OFFSET @deslocamento";
        AdicionarParametro(comando, "@tamanhoPagina", filtro.TamanhoPagina);
        AdicionarParametro(comando, "@deslocamento", (filtro.Pagina - 1) * filtro.TamanhoPagina);

        await using var leitor = await comando.ExecuteReaderAsync();
        var jogos = new List<Jogo>();
        while (await leitor.ReadAsync())
        {
            jogos.Add(Mapear(leitor));
        }
        return (jogos, total);
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"SELECT {Colunas} FROM tb_jogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await using var leitor = await comando.ExecuteReaderAsync();
        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) " +
            "VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque, " +
            "@ativo, @dataCadastro, @idPlataforma, @idCondicaoJogo)");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@dataCadastro", jogo.DataCadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo " +
            "SET no_jogo = @nome, ds_jogo = @descricao, nu_ano_lancamento = @anoLancamento, " +
            "vl_preco = @preco, qt_estoque = @estoque, " +
            "co_plataforma = @idPlataforma, co_condicao_jogo = @idCondicaoJogo " +
            "WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);
        AdicionarParametro(comando, "@id", jogo.Id);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    /// <summary>
    /// Monta o WHERE da listagem e adiciona os parâmetros ao comando. Só entra no SQL
    /// o filtro que foi informado. O texto devolvido contém apenas nomes de coluna e de
    /// parâmetro; os valores vão sempre como parâmetro (RS06).
    /// </summary>
    private static string MontarFiltro(MySqlCommand comando, FiltroListagemJogo filtro)
    {
        var condicoes = new List<string>();

        if (!string.IsNullOrWhiteSpace(filtro.Busca))
        {
            condicoes.Add("no_jogo LIKE CONCAT('%', @busca, '%')");
            AdicionarParametro(comando, "@busca", filtro.Busca.Trim());
        }

        if (filtro.IdPlataforma is not null)
        {
            condicoes.Add("co_plataforma = @idPlataforma");
            AdicionarParametro(comando, "@idPlataforma", filtro.IdPlataforma);
        }

        if (!filtro.IncluirInativos)
        {
            condicoes.Add("st_ativo = TRUE");
        }

        if (filtro.Ids is not null)
        {
            // Lista vazia significa "nenhum jogo atende" (ex.: gênero sem jogos):
            // a condição sempre falsa devolve zero linhas e total 0.
            if (filtro.Ids.Count == 0)
            {
                condicoes.Add("1 = 0");
            }
            else
            {
                var parametrosIds = AdicionarParametrosLista(comando, "@id", filtro.Ids);
                condicoes.Add($"co_seq_jogo IN ({parametrosIds})");
            }
        }

        return condicoes.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", condicoes);
    }

    /// <summary>Converte a linha atual do leitor num objeto Jogo, coluna por coluna.</summary>
    private static Jogo Mapear(MySqlDataReader leitor)
    {
        var ordinalDescricao = leitor.GetOrdinal("ds_jogo");

        return new Jogo
        {
            Id = leitor.GetInt32("co_seq_jogo"),
            Nome = leitor.GetString("no_jogo"),
            Descricao = leitor.IsDBNull(ordinalDescricao) ? null : leitor.GetString(ordinalDescricao),
            AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
            Preco = leitor.GetDecimal("vl_preco"),
            Estoque = leitor.GetInt32("qt_estoque"),
            Ativo = leitor.GetBoolean("st_ativo"),
            DataCadastro = leitor.GetDateTime("dt_cadastro"),
            IdPlataforma = leitor.GetInt32("co_plataforma"),
            IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo"),
        };
    }
}
