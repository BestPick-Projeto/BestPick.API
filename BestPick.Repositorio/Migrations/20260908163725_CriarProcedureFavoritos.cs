using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class CriarProcedureFavoritos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE sp_ListarFavoritosPorUsuario
            @UsuarioId INT
        AS
        BEGIN
            SELECT * FROM Favorito
            WHERE Ativo = 1 AND UsuarioId = @UsuarioId
        END
    ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DROP PROCEDURE sp_ListarFavoritosPorUsuario
    ");
        }
    }
}
