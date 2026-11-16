using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

/// <summary>Conversões entre a entidade Genero e os ApiModels de gênero.</summary>
public static class GeneroMapper
{
    /// <summary>Converte a entidade no formato que sai no JSON.</summary>
    public static ListarGeneroResponseApiModel ParaResponse(this Genero genero)
    {
        return new ListarGeneroResponseApiModel
        {
            Id = genero.Id,
            Nome = genero.Nome,
        };
    }
}
