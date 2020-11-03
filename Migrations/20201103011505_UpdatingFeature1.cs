using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerPlanning.Migrations
{
    public partial class UpdatingFeature1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identification1",
                table: "Feature",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identification1",
                table: "Feature");
        }
    }
}
