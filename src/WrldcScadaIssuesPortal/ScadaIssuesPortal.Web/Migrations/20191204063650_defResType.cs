using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class defResType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(9392),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(9581));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(2142),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(2513));

            migrationBuilder.AlterColumn<string>(
                name: "ResponseType",
                table: "CaseItems",
                nullable: false,
                defaultValue: "ShortText",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "CaseItems",
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
                defaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(9581),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(9392));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 11, 5, 26, 82, DateTimeKind.Local).AddTicks(2513),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(2142));

            migrationBuilder.AlterColumn<string>(
                name: "ResponseType",
                table: "CaseItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValue: "ShortText");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "CaseItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
