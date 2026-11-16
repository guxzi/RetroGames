namespace RetroGames.WebApi.ApiModels;

/// <summary>Uma página de resultados: os itens e os totais para montar a paginação.</summary>
/// <typeparam name="T">Tipo de cada item da página.</typeparam>
public class PaginaResponseApiModel<T>
{
    public List<T> Itens { get; set; } = new();

    public int Pagina { get; set; }

    public int TamanhoPagina { get; set; }

    /// <summary>Total de registros que atendem ao filtro (em todas as páginas).</summary>
    public int TotalItens { get; set; }

    public int TotalPaginas { get; set; }
}
