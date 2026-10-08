using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class TrocarSpPorView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_ListarHistoricoPorUsuario");

            migrationBuilder.Sql(@"
                CREATE VIEW vw_Historico
                AS
                SELECT * FROM HistoricoBusca
                WHERE Ativo = 1
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS vw_Historico");

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
    }
}