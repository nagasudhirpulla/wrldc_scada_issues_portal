using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class caseItemTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_CaseItems_CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.DropTable(
                name: "CaseItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 23, 4, 232, DateTimeKind.Local).AddTicks(1004),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(9392));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 23, 4, 231, DateTimeKind.Local).AddTicks(3538),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(2142));

            migrationBuilder.AddColumn<int>(
                name: "CaseItemTemplateId",
                table: "CaseItemOptions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CaseItemTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SerialNum = table.Column<int>(nullable: false, defaultValue: 1),
                    Question = table.Column<string>(nullable: false),
                    ResponseType = table.Column<string>(nullable: false, defaultValue: "ShortText"),
                    IsResponseRequired = table.Column<bool>(nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseItemTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportingCaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<int>(nullable: false),
                    SerialNum = table.Column<int>(nullable: false, defaultValue: 1),
                    Question = table.Column<string>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    ResponseType = table.Column<string>(nullable: false, defaultValue: "ShortText")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingCaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportingCaseItems_ReportingCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "ReportingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_CaseItemTemplateId",
                table: "CaseItemOptions",
                column: "CaseItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemTemplates_Question",
                table: "CaseItemTemplates",
                column: "Question",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseItems_CaseId",
                table: "ReportingCaseItems",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseItems_Question",
                table: "ReportingCaseItems",
                column: "Question",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_ReportingCaseItems_CaseItemId",
                table: "CaseItemOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseItemOptions_CaseItemTemplates_CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.DropTable(
                name: "CaseItemTemplates");

            migrationBuilder.DropTable(
                name: "ReportingCaseItems");

            migrationBuilder.DropIndex(
                name: "IX_CaseItemOptions_CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.DropColumn(
                name: "CaseItemTemplateId",
                table: "CaseItemOptions");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(9392),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 23, 4, 232, DateTimeKind.Local).AddTicks(1004));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 4, 12, 6, 49, 852, DateTimeKind.Local).AddTicks(2142),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 4, 12, 23, 4, 231, DateTimeKind.Local).AddTicks(3538));

            migrationBuilder.CreateTable(
                name: "CaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<int>(type: "integer", nullable: false),
                    IsResponseRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Question = table.Column<string>(type: "text", nullable: false),
                    Response = table.Column<string>(type: "text", nullable: true),
                    ResponseType = table.Column<string>(type: "text", nullable: false, defaultValue: "ShortText"),
                    SerialNum = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseItems_ReportingCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "ReportingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_CaseId",
                table: "CaseItems",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_Question",
                table: "CaseItems",
                column: "Question",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseItemOptions_CaseItems_CaseItemId",
                table: "CaseItemOptions",
                column: "CaseItemId",
                principalTable: "CaseItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
