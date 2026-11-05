using RetroGames.Model.Model;

namespace RetroGames.Repository;

/// <summary>Acesso aos dados da tabela tb_jogo.</summary>
public interface IJogoRepository
{
    /// <summary>
    /// Os jogos da página pedida e o total de jogos que atendem ao filtro.
    /// Filtro, ordenação, paginação e contagem são feitos no SQL.
    /// </summary>
    Task<(List<Jogo> Jogos, int Total)> ListarAsync(FiltroListagemJogo filtro);

    /// <summary>O jogo com o id informado, ou <c>null</c> se não existir.</summary>
    Task<Jogo?> BuscarPorIdAsync(int id);

    /// <summary>Grava um novo jogo e devolve o id gerado pelo banco.</summary>
    Task<int> InserirAsync(Jogo jogo);

    /// <summary>Atualiza os dados do jogo (não altera st_ativo nem dt_cadastro).</summary>
    Task AlterarAsync(Jogo jogo);

    /// <summary>Marca o jogo como inativo (st_ativo = FALSE). Jogo nunca é apagado (RN03).</summary>
    Task DesativarAsync(int id);
}
