using Microsoft.EntityFrameworkCore.Migrations;

namespace FYPAllocationTest.Migrations
{
    public partial class AreaCodeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "area_code",
                table: "supervisor_area",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "area_code",
                table: "supervisor_area");
        }
    }
}
