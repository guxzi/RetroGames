using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares;

/// <summary>
/// Ponto único que transforma exceções não tratadas em resposta HTTP, sempre no
/// formato do envelope. Por isso nenhum controller precisa de try/catch.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private const string MensagemErroInesperado =
        "Ocorreu um erro inesperado. Tente novamente mais tarde.";

    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        RetroGamesResponseApiModel resposta;

        if (exception is BusinessRuleException erroNegocio)
        {
            // Regra de negócio violada: status e código vêm do catálogo (BusinessErrors).
            httpContext.Response.StatusCode = (int)erroNegocio.Erro.Status;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = erroNegocio.Erro.Code,
                ErrorMessage = erroNegocio.Message,
            };
        }
        else
        {
            // Erro inesperado: o detalhe completo vai só para o log (terminal da API);
            // o cliente recebe uma mensagem genérica, sem SQL, tabela ou caminho de arquivo.
            _logger.LogError(exception, "Erro não tratado em {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERRO,
                ErrorMessage = MensagemErroInesperado,
            };
        }

        await httpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);
        return true;
    }
}
