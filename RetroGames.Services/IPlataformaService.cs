using RetroGames.Model.Model;

namespace RetroGames.Services;

/// <summary>Regras de negócio das plataformas (RN01 e RN02).</summary>
public interface IPlataformaService
{
    /// <summary>Todas as plataformas, como o Repository entrega.</summary>
    Task<List<Plataforma>> ListarAsync();

    /// <summary>A plataforma do id informado. Se não existir: PlataformaNaoEncontrada.</summary>
    Task<Plataforma> BuscarPorIdAsync(int id);

    /// <summary>Grava uma nova plataforma e a devolve com o id gerado.</summary>
    Task<Plataforma> InserirAsync(Plataforma plataforma);

    /// <summary>Altera uma plataforma existente e a devolve já gravada.</summary>
    Task<Plataforma> AlterarAsync(Plataforma plataforma);

    /// <summary>Exclui uma plataforma que não tem jogos.</summary>
    Task ExcluirAsync(int id);
}
