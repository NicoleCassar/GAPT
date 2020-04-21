using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FYPAllocationTest.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    average = table.Column<int>(nullable: false),
                    pref1 = table.Column<string>(nullable: true),
                    pref2 = table.Column<string>(nullable: true),
                    pref3 = table.Column<string>(nullable: true),
                    pref4 = table.Column<string>(nullable: true),
                    pref5 = table.Column<string>(nullable: true),
                    pref6 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentData");
        }
    }
}
