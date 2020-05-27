using Microsoft.EntityFrameworkCore.Migrations;

namespace FYPAllocationTest.Migrations
{
    public partial class AreaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "required_prerequisities",
                table: "supervisor_area");

            migrationBuilder.AddColumn<string>(
                name: "required_prerequisites",
                table: "supervisor_area",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "required_prerequisites",
                table: "supervisor_area");

            migrationBuilder.AddColumn<string>(
                name: "required_prerequisities",
                table: "supervisor_area",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
