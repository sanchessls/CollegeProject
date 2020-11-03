using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerPlanning.Migrations
{
    public partial class UpdatingFeature2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identification1",
                table: "Feature");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identification1",
                table: "Feature",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
