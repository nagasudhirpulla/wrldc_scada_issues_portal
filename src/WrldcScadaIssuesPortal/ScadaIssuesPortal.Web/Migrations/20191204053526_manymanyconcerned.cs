using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class manymanyconcerned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCases_AspNetUsers_ConcernedAgencyId",
                table: "ReportingCases");

            migrationBuilder.DropIndex(
                name: "IX_ReportingCases_ConcernedAgencyId",
                table: "ReportingCases");

            migrationBuilder.DropColumn(
                name: "ConcernedAgencyId",
                table: "ReportingCases");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(9581),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 671, DateTimeKind.Local).AddTicks(1226));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(2513),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 669, DateTimeKind.Local).AddTicks(5213));

            migrationBuilder.AddColumn<int>(
                name: "SerialNum",
                table: "CaseItemOptions",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "ReportingCaseConcerned",
                columns: table => new
                {
                    ReportingCaseId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingCaseConcerned", x => new { x.ReportingCaseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ReportingCaseConcerned_ReportingCases_ReportingCaseId",
                        column: x => x.ReportingCaseId,
                        principalTable: "ReportingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportingCaseConcerned_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseConcerned_UserId",
                table: "ReportingCaseConcerned",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportingCaseConcerned");

            migrationBuilder.DropColumn(
                name: "SerialNum",
                table: "CaseItemOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 671, DateTimeKind.Local).AddTicks(1226),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(9581));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 669, DateTimeKind.Local).AddTicks(5213),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(2513));

            migrationBuilder.AddColumn<string>(
                name: "ConcernedAgencyId",
                table: "ReportingCases",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCases_ConcernedAgencyId",
                table: "ReportingCases",
                column: "ConcernedAgencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCases_AspNetUsers_ConcernedAgencyId",
                table: "ReportingCases",
                column: "ConcernedAgencyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
