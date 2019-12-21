using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class commentsWithTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRemarks",
                table: "ReportingCases");

            migrationBuilder.DropColumn(
                name: "ResolutionRemarks",
                table: "ReportingCases");

            migrationBuilder.CreateTable(
                name: "ReportingCaseComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModified = table.Column<DateTime>(nullable: true),
                    ReportingCaseId = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: false),
                    Tag = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingCaseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportingCaseComments_ReportingCases_ReportingCaseId",
                        column: x => x.ReportingCaseId,
                        principalTable: "ReportingCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseComments_ReportingCaseId",
                table: "ReportingCaseComments",
                column: "ReportingCaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportingCaseComments");

            migrationBuilder.AddColumn<string>(
                name: "AdminRemarks",
                table: "ReportingCases",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolutionRemarks",
                table: "ReportingCases",
                type: "text",
                nullable: true);
        }
    }
}
