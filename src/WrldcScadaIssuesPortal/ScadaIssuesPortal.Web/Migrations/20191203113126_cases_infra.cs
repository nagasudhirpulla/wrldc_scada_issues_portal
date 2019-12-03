using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class cases_infra : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportingCases",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConcernedAgencyId = table.Column<string>(nullable: true),
                    ResolutionTime = table.Column<DateTime>(nullable: false),
                    ResolutionRemarks = table.Column<string>(nullable: true),
                    AdminRemarks = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 982, DateTimeKind.Local).AddTicks(4225)),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValue: new DateTime(2019, 12, 3, 17, 1, 25, 984, DateTimeKind.Local).AddTicks(3007))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportingCases_AspNetUsers_ConcernedAgencyId",
                        column: x => x.ConcernedAgencyId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyItems",
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptionText = table.Column<string>(nullable: true),
                    SurveyItemId = table.Column<int>(nullable: false)
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
                name: "IX_ReportingCases_ConcernedAgencyId",
                table: "ReportingCases",
                column: "ConcernedAgencyId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyOptions");

            migrationBuilder.DropTable(
                name: "SurveyItems");

            migrationBuilder.DropTable(
                name: "ReportingCases");
        }
    }
}
