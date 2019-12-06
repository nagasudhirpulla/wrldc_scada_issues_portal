using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class createdAtUpdatedAtCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 850, DateTimeKind.Local).AddTicks(5492));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValueSql: "NOW()",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 849, DateTimeKind.Local).AddTicks(8430));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 850, DateTimeKind.Local).AddTicks(5492),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "NOW()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 5, 15, 50, 15, 849, DateTimeKind.Local).AddTicks(8430),
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "NOW()");
        }
    }
}
