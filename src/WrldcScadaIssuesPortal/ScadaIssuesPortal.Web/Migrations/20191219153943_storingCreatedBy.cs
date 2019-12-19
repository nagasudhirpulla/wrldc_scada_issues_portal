using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class storingCreatedBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ReportingCases",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCases_CreatedById",
                table: "ReportingCases",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCases_AspNetUsers_CreatedById",
                table: "ReportingCases",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCases_AspNetUsers_CreatedById",
                table: "ReportingCases");

            migrationBuilder.DropIndex(
                name: "IX_ReportingCases_CreatedById",
                table: "ReportingCases");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ReportingCases");
        }
    }
}
