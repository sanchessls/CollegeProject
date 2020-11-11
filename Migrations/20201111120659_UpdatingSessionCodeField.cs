using Microsoft.EntityFrameworkCore.Migrations;

namespace ScrumPokerPlanning.Migrations
{
    public partial class UpdatingSessionCodeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionCode",
                table: "PlanningSession",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionCode",
                table: "PlanningSession");
        }
    }
}
