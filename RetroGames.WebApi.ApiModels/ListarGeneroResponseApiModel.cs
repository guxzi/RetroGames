namespace RetroGames.WebApi.ApiModels;

/// <summary>Os dados de um gênero que saem no JSON.</summary>
public class ListarGeneroResponseApiModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
}
