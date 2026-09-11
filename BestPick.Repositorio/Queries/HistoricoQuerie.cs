namespace BestPick.Repositorio.Queries;

public static class HistoricoQueries
{
    public const string CriarProcedureListarPorUsuario = @"
        CREATE PROCEDURE sp_ListarHistoricoPorUsuario
            @UsuarioId INT
        AS
        BEGIN
            SELECT * FROM HistoricoBusca
            WHERE Ativo = 1 AND UsuarioId = @UsuarioId
            ORDER BY DataBusca DESC
        END
    ";

    public const string ListarPorUsuario = "EXEC sp_ListarHistoricoPorUsuario @UsuarioId";
}