namespace BestPick.Repositorio.Functions;

public static class DashboardFunctions
{
    public const string CriarFunctionCalcularPorcentagem = @"
        CREATE FUNCTION fn_CalcularPorcentagem
        (
            @Quantidade INT,
            @Total INT
        )
        RETURNS DECIMAL(5,2)
        AS
        BEGIN
            IF @Total = 0
                RETURN 0;

            RETURN (@Quantidade * 100.0) / @Total;
        END
    ";

    public const string CalcularPorcentagem =
        "SELECT dbo.fn_CalcularPorcentagem(@Quantidade, @Total)";
}