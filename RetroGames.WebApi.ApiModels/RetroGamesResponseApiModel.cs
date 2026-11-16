using System.Text.Json.Serialization;

namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// Envelope de toda resposta da API, sem dados: usado nas respostas de erro, que
/// omitem a propriedade data. JSON: { "responseCode": 100, "errorMessage": "..." }
/// </summary>
public class RetroGamesResponseApiModel
{
    /// <summary>Código da resposta: 0 (SUCESSO), 1 (ERRO) ou o código do erro de negócio.</summary>
    [JsonPropertyOrder(1)]
    public int ResponseCode { get; set; }

    /// <summary>Mensagem de erro; <c>null</c> quando deu certo.</summary>
    [JsonPropertyOrder(3)]
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Envelope com dados. JSON: { "responseCode": 0, "data": { ... }, "errorMessage": null }
/// </summary>
/// <typeparam name="T">Tipo do ApiModel que vai em data.</typeparam>
public class RetroGamesResponseApiModel<T> : RetroGamesResponseApiModel
{
    /// <summary>Os dados da resposta (um ApiModel ou uma lista de ApiModels).</summary>
    [JsonPropertyOrder(2)]
    public T? Data { get; set; }
}
