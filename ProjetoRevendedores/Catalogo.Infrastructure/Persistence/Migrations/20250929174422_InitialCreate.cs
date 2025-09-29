using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalogo.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Setores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Ordem = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrecoCentavos = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    SetorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IconeUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Setores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "Setores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Setores",
                columns: new[] { "Id", "Ativo", "Nome", "Ordem" },
                values: new object[,]
                {
                    { new Guid("2a31e6a4-3e10-4c9b-bd14-752f2824c73f"), true, "Acessórios", 4 },
                    { new Guid("7f2ad0df-9ea1-4d0e-8cd7-fb5d714d4b01"), true, "Masculino", 1 },
                    { new Guid("d214c62b-e16a-4b34-9d7a-5e2e2d98eabd"), true, "Infantil", 3 },
                    { new Guid("f9fb24d3-7c2e-4b22-8a57-36262dd3cb84"), true, "Feminino", 2 }
                });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "IconeUrl", "Nome", "PrecoCentavos", "SetorId" },
                values: new object[,]
                {
                    { new Guid("3565cc3a-652e-4b72-919d-58c81551979c"), true, "/icons/camiseta-infantil.png", "Camiseta Super-Herói", 3990, new Guid("d214c62b-e16a-4b34-9d7a-5e2e2d98eabd") },
                    { new Guid("3f5a8c9e-b781-4af1-9bfb-7a3d67cfec21"), true, "/icons/camiseta.png", "Camiseta Branca", 4990, new Guid("7f2ad0df-9ea1-4d0e-8cd7-fb5d714d4b01") },
                    { new Guid("530f8d05-0b42-4838-8f47-2e9938c1570f"), true, "/icons/blusa.png", "Blusa Seda Preta", 8990, new Guid("f9fb24d3-7c2e-4b22-8a57-36262dd3cb84") },
                    { new Guid("7292e04f-74b6-4823-8c38-d2ff37b18d70"), true, "/icons/jaqueta-infantil.png", "Jaqueta Jeans Infantil", 10990, new Guid("d214c62b-e16a-4b34-9d7a-5e2e2d98eabd") },
                    { new Guid("7b6c4622-7d1e-4ea4-80c8-861ad9381abd"), true, "/icons/vestido.png", "Vestido Floral", 15990, new Guid("f9fb24d3-7c2e-4b22-8a57-36262dd3cb84") },
                    { new Guid("86ff2c54-3d52-4627-93d0-61aafb489a89"), true, "/icons/cinto.png", "Cinto de Couro", 7990, new Guid("2a31e6a4-3e10-4c9b-bd14-752f2824c73f") },
                    { new Guid("ad208ef7-7c0e-4f41-bc49-e18cc26be3b9"), true, "/icons/bone.png", "Boné Preto", 5990, new Guid("2a31e6a4-3e10-4c9b-bd14-752f2824c73f") },
                    { new Guid("af0fdc59-5fd5-4cd1-a3bf-2cfdf5e2ddc9"), true, "/icons/mochila.png", "Mochila Casual", 14990, new Guid("2a31e6a4-3e10-4c9b-bd14-752f2824c73f") },
                    { new Guid("c1a890d6-79ab-4df0-9fa5-dcdaf6c7d429"), true, "/icons/legging.png", "Legging Rosa", 5990, new Guid("d214c62b-e16a-4b34-9d7a-5e2e2d98eabd") },
                    { new Guid("c32eb407-794b-47e7-8b94-825349f4281e"), true, "/icons/jaqueta.png", "Jaqueta Moletom Cinza", 18990, new Guid("7f2ad0df-9ea1-4d0e-8cd7-fb5d714d4b01") },
                    { new Guid("f0b3a91c-0ce7-40dd-9aa5-b3ad59f56076"), true, "/icons/calca.png", "Calça Jeans Slim Azul", 13990, new Guid("7f2ad0df-9ea1-4d0e-8cd7-fb5d714d4b01") },
                    { new Guid("f9a01a83-2a16-4e30-b728-13475a0e5ca4"), true, "/icons/saia.png", "Saia Jeans", 11990, new Guid("f9fb24d3-7c2e-4b22-8a57-36262dd3cb84") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_SetorId",
                table: "Produtos",
                column: "SetorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Setores");
        }
    }
}
