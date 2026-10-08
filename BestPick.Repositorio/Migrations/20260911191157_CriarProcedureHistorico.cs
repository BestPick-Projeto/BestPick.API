using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class CriarProcedureHistorico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_ListarHistoricoPorUsuario
                    @UsuarioId INT
                AS
                BEGIN
                    SELECT * FROM HistoricoBusca
                    WHERE Ativo = 1 AND UsuarioId = @UsuarioId
                    ORDER BY DataBusca DESC
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_ListarHistoricoPorUsuario");
        }
    }
}