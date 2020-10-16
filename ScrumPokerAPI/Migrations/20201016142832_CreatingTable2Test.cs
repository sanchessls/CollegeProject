using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerAPI.Migrations
{
    public partial class CreatingTable2Test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTwo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HowTo2 = table.Column<string>(nullable: false),
                    Line2 = table.Column<string>(nullable: false),
                    Plataform2 = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTwo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTwo");
        }
    }
}
