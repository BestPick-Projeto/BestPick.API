using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestPick.Repositorio.Migrations
{
    public partial class RemoverMaleficiosProduto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Maleficios",
                table: "Produto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Maleficios",
                table: "Produto",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
