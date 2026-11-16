namespace RetroGames.WebApi.ApiModels;

/// <summary>
/// Catálogo dos códigos da própria API (responseCode do envelope). Os erros de negócio
/// (100 em diante) ficam no BusinessErrors, no projeto Services.
/// </summary>
public static class ResponseCodes
{
    /// <summary>A requisição foi atendida.</summary>
    public const int SUCESSO = 0;

    /// <summary>Erro inesperado; a mensagem genérica é definida no GlobalExceptionHandler.</summary>
    public const int ERRO = 1;
}
