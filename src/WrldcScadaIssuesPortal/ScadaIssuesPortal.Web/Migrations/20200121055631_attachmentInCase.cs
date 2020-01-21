using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class attachmentInCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AttachmentFilename",
                table: "ReportingCases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentFilename",
                table: "ReportingCases");
        }
    }
}
