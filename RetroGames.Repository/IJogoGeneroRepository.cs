using RetroGames.Model.Model;

namespace RetroGames.Repository;

/// <summary>Acesso aos dados da tabela associativa tb_jogo_genero.</summary>
public interface IJogoGeneroRepository
{
    /// <summary>Todos os vínculos entre jogos e gêneros.</summary>
    Task<List<JogoGenero>> ListarTodosAsync();

    /// <summary>Os vínculos dos jogos informados, numa única consulta.</summary>
    Task<List<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogos);

    /// <summary>Grava os vínculos de um jogo com os seus gêneros.</summary>
    Task InserirAsync(int idJogo, IReadOnlyCollection<int> idsGeneros);

    /// <summary>Apaga os vínculos de um jogo (usado antes de regravá-los na alteração).</summary>
    Task RemoverPorJogoAsync(int idJogo);
}
