using RetroGames.Repository;
using RetroGames.Services;

namespace RetroGames.WebApi.Configuration;

/// <summary>
/// Registro das classes na injeção de dependência. O Program.cs chama estes dois
/// métodos; assim, quem precisa de um Service ou Repository recebe pela interface.
/// </summary>
public static class InjecaoDependenciaConfig
{
    /// <summary>
    /// Registra a configuração do Repository (uma única vez para a aplicação toda),
    /// cada Repository e o RepositorySet (um objeto novo por requisição).
    /// </summary>
    public static IServiceCollection AddRepositorios(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString });

        services.AddScoped<IPlataformaRepository, PlataformaRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IJogoGeneroRepository, JogoGeneroRepository>();
        services.AddScoped<IGeneroRepository, GeneroRepository>();
        services.AddScoped<ICondicaoJogoRepository, CondicaoJogoRepository>();
        services.AddScoped<IRepositorySet, RepositorySet>();

        return services;
    }

    /// <summary>Registra cada Service com a sua interface (um objeto novo por requisição).</summary>
    public static IServiceCollection AddServicosDeNegocio(this IServiceCollection services)
    {
        services.AddScoped<IPlataformaService, PlataformaService>();
        services.AddScoped<IJogoService, JogoService>();
        services.AddScoped<IGeneroService, GeneroService>();
        services.AddScoped<ICondicaoJogoService, CondicaoJogoService>();

        return services;
    }
}
