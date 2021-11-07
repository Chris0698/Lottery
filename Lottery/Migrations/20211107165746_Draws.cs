using Microsoft.EntityFrameworkCore.Migrations;

namespace Lottery.Migrations
{
    public partial class Draws : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Draw",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrawnNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    PrizeMoney = table.Column<int>(type: "int", nullable: false),
                    SelectedGame = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draw", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Draw");
        }
    }
}
