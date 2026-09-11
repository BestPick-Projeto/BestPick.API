namespace BestPick.Repositorio.Queries;

public static class ProdutoQueries
{
    public const string CriarProcedureBuscarPorHistorico = @"
        CREATE PROCEDURE sp_BuscarProdutosPorHistorico
            @HistoricoId INT
        AS
        BEGIN
            SELECT * FROM Produto
            WHERE HistoricoBuscaId = @HistoricoId
        END
    ";

    public const string BuscarPorHistorico = "EXEC sp_BuscarProdutosPorHistorico @HistoricoId";
}
