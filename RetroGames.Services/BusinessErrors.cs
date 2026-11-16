using System.Net;

namespace RetroGames.Services;

/// <summary>
/// Catálogo dos erros de negócio. Cada erro é definido uma única vez, aqui, e é
/// usado assim: <c>throw new BusinessRuleException(BusinessErrors.PlataformaNaoEncontrada)</c>.
/// </summary>
public static class BusinessErrors
{
    /// <summary>A plataforma pedida, ou a informada no jogo, não existe (RN05).</summary>
    public static readonly BusinessError PlataformaNaoEncontrada =
        new(100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    /// <summary>Já existe plataforma com o mesmo nome (RN01).</summary>
    public static readonly BusinessError PlataformaNomeDuplicado =
        new(101, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    /// <summary>Exclusão de plataforma que tem jogos vinculados (RN02).</summary>
    public static readonly BusinessError PlataformaPossuiJogos =
        new(102, "Não é possível excluir: existem jogos desta plataforma.", HttpStatusCode.Conflict);

    /// <summary>O id do jogo não existe.</summary>
    public static readonly BusinessError JogoNaoEncontrado =
        new(103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    /// <summary>A condição informada no jogo não existe (RN05).</summary>
    public static readonly BusinessError CondicaoNaoEncontrada =
        new(104, "A condição informada não existe.", HttpStatusCode.NotFound);

    /// <summary>Um ou mais gêneros informados no jogo não existem (RN05).</summary>
    public static readonly BusinessError GeneroNaoEncontrado =
        new(105, "Um ou mais gêneros informados não existem.", HttpStatusCode.NotFound);
}
