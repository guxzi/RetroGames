namespace RetroGames.Model.Model;

/// <summary>Perfil de acesso do usuário (tabela tb_perfil).</summary>
public class Perfil
{
    /// <summary>co_perfil (chave fixa, sem AUTO_INCREMENT)</summary>
    public int Id { get; set; }

    /// <summary>ds_perfil</summary>
    public string Descricao { get; set; } = string.Empty;
}
