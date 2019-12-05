using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class caseItemUniqueQuesRemove : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReportingCaseItems_Question",
                table: "ReportingCaseItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 850, DateTimeKind.Local).AddTicks(5492),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 5, 10, 18, 58, 832, DateTimeKind.Local).AddTicks(4431));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 849, DateTimeKind.Local).AddTicks(8430),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 5, 10, 18, 58, 831, DateTimeKind.Local).AddTicks(7380));

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "CaseItemOptions",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 10, 18, 58, 832, DateTimeKind.Local).AddTicks(4431),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 850, DateTimeKind.Local).AddTicks(5492));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 10, 18, 58, 831, DateTimeKind.Local).AddTicks(7380),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 849, DateTimeKind.Local).AddTicks(8430));

            migrationBuilder.AlterColumn<string>(
                name: "OptionText",
                table: "CaseItemOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseItems_Question",
                table: "ReportingCaseItems",
                column: "Question",
                unique: true);
        }
    }
}
