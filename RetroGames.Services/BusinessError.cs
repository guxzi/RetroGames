using System.Net;

namespace RetroGames.Services;

/// <summary>
/// Um erro de negócio do catálogo: código (a partir de 100), mensagem e o status HTTP
/// com que a API vai responder. O Service só guarda o valor do status; quem monta a
/// resposta HTTP é o GlobalExceptionHandler da API.
/// </summary>
/// <param name="Code">Código devolvido em responseCode.</param>
/// <param name="Message">Mensagem devolvida em errorMessage.</param>
/// <param name="Status">Status HTTP da resposta (400, 404 ou 409).</param>
public sealed record BusinessError(int Code, string Message, HttpStatusCode Status);
