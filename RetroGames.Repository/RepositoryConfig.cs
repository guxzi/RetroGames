namespace RetroGames.Repository;

/// <summary>
/// Configuração do Repository: guarda a connection string do MySQL.
/// O valor é preenchido pela API (Aula 4), que o lê do arquivo .env.
/// </summary>
public class RepositoryConfig
{
    public string ConnectionString { get; set; } = string.Empty;
}
