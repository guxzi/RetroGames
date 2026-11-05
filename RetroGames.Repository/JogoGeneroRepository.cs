using MySqlConnector;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

/// <summary>Todo o SQL da tabela tb_jogo_genero.</summary>
public class JogoGeneroRepository : RepositoryBase, IJogoGeneroRepository
{
    private const string Colunas = "co_jogo, co_genero";

    public JogoGeneroRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            $"SELECT {Colunas} FROM tb_jogo_genero ORDER BY co_jogo, co_genero");

        return await LerVinculosAsync(comando);
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogos)
    {
        // Sem ids, não há o que buscar (e "IN ()" seria SQL inválido).
        if (idsJogos.Count == 0)
        {
            return new List<JogoGenero>();
        }

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, string.Empty);
        var parametrosIds = AdicionarParametrosLista(comando, "@idJogo", idsJogos);
        comando.CommandText =
            $"SELECT {Colunas} FROM tb_jogo_genero " +
            $"WHERE co_jogo IN ({parametrosIds}) " +
            "ORDER BY co_jogo, co_genero";

        return await LerVinculosAsync(comando);
    }

    public async Task InserirAsync(int idJogo, IReadOnlyCollection<int> idsGeneros)
    {
        if (idsGeneros.Count == 0)
        {
            return;
        }

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, string.Empty);
        AdicionarParametro(comando, "@idJogo", idJogo);

        // Um INSERT com várias linhas: VALUES (@idJogo, @idGenero0), (@idJogo, @idGenero1), ...
        var linhas = new List<string>();
        foreach (var idGenero in idsGeneros)
        {
            var nomeParametro = $"@idGenero{linhas.Count}";
            AdicionarParametro(comando, nomeParametro, idGenero);
            linhas.Add($"(@idJogo, {nomeParametro})");
        }
        comando.CommandText =
            "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES " + string.Join(", ", linhas);

        // A PK composta impede vínculo repetido: o erro 1062 vira RegistroDuplicadoException.
        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo");
        AdicionarParametro(comando, "@idJogo", idJogo);

        await comando.ExecuteNonQueryAsync();
    }

    /// <summary>Executa a consulta e converte cada linha num JogoGenero.</summary>
    private static async Task<List<JogoGenero>> LerVinculosAsync(MySqlCommand comando)
    {
        await using var leitor = await comando.ExecuteReaderAsync();
        var vinculos = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            vinculos.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero"),
            });
        }
        return vinculos;
    }
}
