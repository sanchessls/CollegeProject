using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerPlanning.Migrations
{
    public partial class LinqIntoJiraFeature2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "JiraCreated",
                table: "Feature",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JiraCreated",
                table: "Feature");
        }
    }
}
