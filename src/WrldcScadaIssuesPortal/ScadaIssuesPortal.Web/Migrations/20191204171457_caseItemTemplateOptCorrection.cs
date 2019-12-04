using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class caseItemTemplateOptCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_ReportingCaseItems_CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_CaseItemTemplates_CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_CaseItemOptions_CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_CaseItemOptions_OptionText_CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.DropColumn(
                name: "CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 22, 44, 56, 514, DateTimeKind.Local).AddTicks(6030),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 59, 0, 623, DateTimeKind.Local).AddTicks(3623));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 22, 44, 56, 513, DateTimeKind.Local).AddTicks(3149),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 59, 0, 622, DateTimeKind.Local).AddTicks(6376));

            migrationBuilder.AlterColumn<int>(
                name: "CaseItemTemplateId",
                table: "CaseItemOptions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_OptionText_CaseItemTemplateId",
                table: "CaseItemOptions",
                columns: new[] { "OptionText", "CaseItemTemplateId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItemOptions_CaseItemTemplates_CaseItemTemplateId",
                table: "CaseItemOptions",
                column: "CaseItemTemplateId",
                principalTable: "CaseItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_CaseItemTemplates_CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.DropIndex(
                name: "IX_CaseItemOptions_OptionText_CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 59, 0, 623, DateTimeKind.Local).AddTicks(3623),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 22, 44, 56, 514, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 59, 0, 622, DateTimeKind.Local).AddTicks(6376),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 22, 44, 56, 513, DateTimeKind.Local).AddTicks(3149));

            migrationBuilder.AlterColumn<int>(
                name: "CaseItemTemplateId",
                table: "CaseItemOptions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CaseItemId",
                table: "CaseItemOptions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_CaseItemId",
                table: "CaseItemOptions",
                column: "CaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_OptionText_CaseItemId",
                table: "CaseItemOptions",
                columns: new[] { "OptionText", "CaseItemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItemOptions_ReportingCaseItems_CaseItemId",
                table: "CaseItemOptions",
                column: "CaseItemId",
                principalTable: "ReportingCaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItemOptions_CaseItemTemplates_CaseItemTemplateId",
                table: "CaseItemOptions",
                column: "CaseItemTemplateId",
                principalTable: "CaseItemTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
