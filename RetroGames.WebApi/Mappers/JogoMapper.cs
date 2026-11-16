using RetroGames.Repository;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

/// <summary>
/// Conversões do jogo: o request da listagem vira o filtro do Service, e o
/// JogoDetalheServiceModel vira os ApiModels de saída.
/// </summary>
public static class JogoMapper
{
    /// <summary>
    /// Converte os parâmetros da query string no filtro da listagem. O gênero não entra
    /// aqui: ele vai separado para o Service, que resolve esse filtro com LINQ.
    /// </summary>
    public static FiltroListagemJogo ParaFiltro(this ListarJogoRequestApiModel request)
    {
        return new FiltroListagemJogo
        {
            Busca = request.Busca,
            IdPlataforma = request.IdPlataforma,
            Pagina = request.Pagina,
            TamanhoPagina = request.TamanhoPagina,
        };
    }

    /// <summary>Converte o detalhe do Service num item da listagem.</summary>
    public static ListarJogoResponseApiModel ParaResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            NomePlataforma = detalhe.NomePlataforma,
            DescricaoCondicao = detalhe.DescricaoCondicao,
            Generos = detalhe.Generos.Select(g => g.Nome).ToList(),
        };
    }

    /// <summary>Converte o detalhe do Service no ApiModel de detalhe do jogo.</summary>
    public static ListarPorIdJogoResponseApiModel ParaDetalheResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarPorIdJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            Descricao = detalhe.Jogo.Descricao,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            IdPlataforma = detalhe.Jogo.IdPlataforma,
            NomePlataforma = detalhe.NomePlataforma,
            IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
            DescricaoCondicao = detalhe.DescricaoCondicao,
            Generos = detalhe.Generos.Select(g => g.ParaResponse()).ToList(),
        };
    }

    /// <summary>Monta a página de resultados com os itens e os totais.</summary>
    public static PaginaResponseApiModel<ListarJogoResponseApiModel> ParaPaginaResponse(
        this List<JogoDetalheServiceModel> jogos, int totalItens, int pagina, int tamanhoPagina)
    {
        return new PaginaResponseApiModel<ListarJogoResponseApiModel>
        {
            Itens = jogos.Select(j => j.ParaResponse()).ToList(),
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = totalItens,
            TotalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina),
        };
    }
}
