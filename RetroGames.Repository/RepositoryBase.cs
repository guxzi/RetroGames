using MySqlConnector;

namespace RetroGames.Repository;

/// <summary>
/// Base de todos os repositórios. Concentra o que todo Repository repete:
/// abrir conexão, criar comando, adicionar parâmetro e executar gravação.
/// Não contém SQL de nenhuma tabela.
/// </summary>
/// <remarks>
/// Quem chama estes métodos usa <c>await using</c> na conexão, no comando e no
/// leitor, para que sejam fechados mesmo quando acontece uma exceção.
/// </remarks>
public abstract class RepositoryBase
{
    private readonly RepositoryConfig _config;

    protected RepositoryBase(RepositoryConfig config)
    {
        _config = config;
    }

    /// <summary>Abre uma conexão com o banco usando a connection string configurada.</summary>
    protected async Task<MySqlConnection> AbrirConexaoAsync()
    {
        var conexao = new MySqlConnection(_config.ConnectionString);
        await conexao.OpenAsync();
        return conexao;
    }

    /// <summary>Cria um comando com o texto SQL informado, ligado à conexão.</summary>
    protected static MySqlCommand CriarComando(MySqlConnection conexao, string sql)
    {
        return new MySqlCommand(sql, conexao);
    }

    /// <summary>
    /// Adiciona um parâmetro ao comando. O valor nunca é concatenado no SQL (RS06).
    /// <c>null</c> do C# é convertido em <see cref="DBNull.Value"/>, que é o NULL do banco.
    /// </summary>
    protected static void AdicionarParametro(MySqlCommand comando, string nome, object? valor)
    {
        comando.Parameters.AddWithValue(nome, valor ?? DBNull.Value);
    }

    /// <summary>
    /// Adiciona um parâmetro para cada valor da lista (@prefixo0, @prefixo1, ...) e devolve
    /// os nomes separados por vírgula, prontos para um <c>IN (...)</c>. Necessário porque
    /// <c>IN (@lista)</c> não aceita uma lista inteira num único parâmetro.
    /// O SQL recebe só os NOMES dos parâmetros; os valores continuam como parâmetro.
    /// </summary>
    protected static string AdicionarParametrosLista(MySqlCommand comando, string prefixo, IEnumerable<int> valores)
    {
        var nomes = new List<string>();
        foreach (var valor in valores)
        {
            var nome = $"{prefixo}{nomes.Count}";
            AdicionarParametro(comando, nome, valor);
            nomes.Add(nome);
        }
        return string.Join(", ", nomes);
    }

    /// <summary>
    /// Executa uma gravação (INSERT ou UPDATE) e devolve o número de linhas afetadas.
    /// É o único lugar que converte o erro 1062 (valor repetido em coluna UNIQUE)
    /// em <see cref="RegistroDuplicadoException"/>.
    /// </summary>
    protected static async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            return await comando.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            throw new RegistroDuplicadoException("Já existe um registro com este valor.", ex);
        }
    }
}
