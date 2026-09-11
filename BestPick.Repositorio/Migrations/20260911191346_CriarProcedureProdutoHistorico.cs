using BestPick.Repositorio.Queries;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class CriarProcedureProdutoHistorico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ProdutoQueries.CriarProcedureBuscarPorHistorico);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE sp_BuscarProdutosPorHistorico");
        }
    }
}
