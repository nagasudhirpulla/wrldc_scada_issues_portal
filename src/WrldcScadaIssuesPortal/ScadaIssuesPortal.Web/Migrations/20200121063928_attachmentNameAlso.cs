using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class attachmentNameAlso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFilename",
                table: "ReportingCases");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "ReportingCases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                table: "ReportingCases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "ReportingCases");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                table: "ReportingCases");

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFilename",
                table: "ReportingCases",
                type: "text",
                nullable: true);
        }
    }
}
