using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FYPAllocationTest.Migrations
{
    public partial class AllocationMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    student_id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    average_mark = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student", x => x.student_id);
                });

            migrationBuilder.CreateTable(
                name: "supervisor",
                columns: table => new
                {
                    supervisor_id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    quota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supervisor", x => x.supervisor_id);
                });

            migrationBuilder.CreateTable(
                name: "allocation",
                columns: table => new
                {
                    allocation_id = table.Column<int>(nullable: false),
                    student_id = table.Column<string>(nullable: true),
                    supervisor_id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allocation", x => x.allocation_id);
                    table.ForeignKey(
                        name: "FK_allocation_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_allocation_supervisor_supervisor_id",
                        column: x => x.supervisor_id,
                        principalTable: "supervisor",
                        principalColumn: "supervisor_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supervisor_area",
                columns: table => new
                {
                    area_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supervisor_id = table.Column<string>(nullable: true),
                    title = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    available = table.Column<bool>(nullable: false),
                    area_quota = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supervisor_area", x => x.area_id);
                    table.ForeignKey(
                        name: "FK_supervisor_area_supervisor_supervisor_id",
                        column: x => x.supervisor_id,
                        principalTable: "supervisor",
                        principalColumn: "supervisor_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "student_preference",
                columns: table => new
                {
                    preference_id = table.Column<int>(nullable: false),
                    student_id = table.Column<string>(nullable: false),
                    area_id = table.Column<int>(nullable: false),
                    time_submitted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_preference", x => new { x.preference_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_student_preference_supervisor_area_area_id",
                        column: x => x.area_id,
                        principalTable: "supervisor_area",
                        principalColumn: "area_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_preference_student_student_id",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_allocation_student_id",
                table: "allocation",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_allocation_supervisor_id",
                table: "allocation",
                column: "supervisor_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_preference_area_id",
                table: "student_preference",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_preference_student_id",
                table: "student_preference",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_supervisor_area_supervisor_id",
                table: "supervisor_area",
                column: "supervisor_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allocation");

            migrationBuilder.DropTable(
                name: "student_preference");

            migrationBuilder.DropTable(
                name: "supervisor_area");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "supervisor");
        }
    }
}
