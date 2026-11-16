using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

/// <summary>Conversões entre a entidade CondicaoJogo e os ApiModels de condição.</summary>
public static class CondicaoJogoMapper
{
    /// <summary>Converte a entidade no formato que sai no JSON.</summary>
    public static ListarCondicaoJogoResponseApiModel ParaResponse(this CondicaoJogo condicao)
    {
        return new ListarCondicaoJogoResponseApiModel
        {
            Id = condicao.Id,
            Descricao = condicao.Descricao,
        };
    }
}
