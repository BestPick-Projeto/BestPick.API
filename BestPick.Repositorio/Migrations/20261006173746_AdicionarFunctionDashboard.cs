using Microsoft.EntityFrameworkCore.Migrations;
using BestPick.Repositorio.Functions;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class AdicionarFunctionDashboard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DashboardFunctions.CriarFunctionCalcularPorcentagem);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS fn_CalcularPorcentagem");
        }
    }
}
