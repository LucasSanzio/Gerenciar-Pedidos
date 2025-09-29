using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reporting.Projections.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendasPorDiaSetorView",
                columns: table => new
                {
                    Data = table.Column<DateTime>(type: "date", nullable: false),
                    SetorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetorNome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TotalVendidoCentavos = table.Column<int>(type: "int", nullable: false),
                    QuantidadePedidos = table.Column<int>(type: "int", nullable: false),
                    QuantidadeItens = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendasPorDiaSetorView", x => new { x.Data, x.SetorId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendasPorDiaSetorView");
        }
    }
}
