using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class case_table_names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyOptions");

            migrationBuilder.DropTable(
                name: "SurveyItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 671, DateTimeKind.Local).AddTicks(1226),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 984, DateTimeKind.Local).AddTicks(3007));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 669, DateTimeKind.Local).AddTicks(5213),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 982, DateTimeKind.Local).AddTicks(4225));

            migrationBuilder.CreateTable(
                name: "CaseItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<int>(nullable: false),
                    SerialNum = table.Column<int>(nullable: false, defaultValue: 1),
                    Question = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    ResponseType = table.Column<string>(nullable: false),
                    IsResponseRequired = table.Column<bool>(nullable: false, defaultValue: true)
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

            migrationBuilder.CreateTable(
                name: "CaseItemOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptionText = table.Column<string>(nullable: true),
                    CaseItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseItemOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CaseItemOptions_CaseItems_CaseItemId",
                        column: x => x.CaseItemId,
                        principalTable: "CaseItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_CaseItemId",
                table: "CaseItemOptions",
                column: "CaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItemOptions_OptionText_CaseItemId",
                table: "CaseItemOptions",
                columns: new[] { "OptionText", "CaseItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_CaseId",
                table: "CaseItems",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseItems_Question",
                table: "CaseItems",
                column: "Question",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseItemOptions");

            migrationBuilder.DropTable(
                name: "CaseItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 984, DateTimeKind.Local).AddTicks(3007),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 671, DateTimeKind.Local).AddTicks(1226));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ReportingCases",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 982, DateTimeKind.Local).AddTicks(4225),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 3, 17, 6, 35, 669, DateTimeKind.Local).AddTicks(5213));

            migrationBuilder.CreateTable(
                name: "SurveyItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<int>(type: "integer", nullable: false),
                    IsResponseRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Question = table.Column<string>(type: "text", nullable: true),
                    Response = table.Column<string>(type: "text", nullable: true),
                    ResponseType = table.Column<string>(type: "text", nullable: false),
                    SerialNum = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyItems_ReportingCases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "ReportingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptionText = table.Column<string>(type: "text", nullable: true),
                    SurveyItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurveyOptions_SurveyItems_SurveyItemId",
                        column: x => x.SurveyItemId,
                        principalTable: "SurveyItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyItems_CaseId",
                table: "SurveyItems",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyItems_Question",
                table: "SurveyItems",
                column: "Question",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOptions_SurveyItemId",
                table: "SurveyOptions",
                column: "SurveyItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyOptions_OptionText_SurveyItemId",
                table: "SurveyOptions",
                columns: new[] { "OptionText", "SurveyItemId" },
                unique: true);
        }
    }
}
