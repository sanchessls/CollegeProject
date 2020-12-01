using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerPlanning.Migrations
{
    public partial class LinqIntoJiraFeature3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Voted",
                table: "FeatureUser",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Voted",
                table: "FeatureUser");
        }
    }
}
