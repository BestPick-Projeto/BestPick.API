namespace BestPick.Repositorio.Queries;

public static class FavoritoQueries
{
    public const string CriarProcedureListarPorUsuario = @"
        CREATE PROCEDURE sp_ListarFavoritosPorUsuario
            @UsuarioId INT
        AS
        BEGIN
            SELECT * FROM Favorito
            WHERE Excluido = 0 AND UsuarioId = @UsuarioId
        END
    ";

    public const string ListarPorUsuario = "EXEC sp_ListarFavoritosPorUsuario @UsuarioId";
}