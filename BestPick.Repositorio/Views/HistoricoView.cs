namespace BestPick.Repositorio.Views;

public static class HistoricoView
{
    public const string CriarView = @"
        CREATE VIEW vw_Historico
        AS
        SELECT * FROM HistoricoBusca
        WHERE Ativo = 1
    ";

    public const string ListarPorUsuario = @"
        SELECT * FROM vw_Historico
        WHERE UsuarioId = @UsuarioId
        ORDER BY DataBusca DESC
    ";
}
