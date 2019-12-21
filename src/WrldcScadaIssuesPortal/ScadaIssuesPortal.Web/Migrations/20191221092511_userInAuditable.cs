using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class userInAuditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ReportingCaseComments");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ReportingCaseComments");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ReportingCaseComments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedById",
                table: "ReportingCaseComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseComments_CreatedById",
                table: "ReportingCaseComments",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReportingCaseComments_LastModifiedById",
                table: "ReportingCaseComments",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseComments_AspNetUsers_CreatedById",
                table: "ReportingCaseComments",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseComments_AspNetUsers_LastModifiedById",
                table: "ReportingCaseComments",
                column: "LastModifiedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseComments_AspNetUsers_CreatedById",
                table: "ReportingCaseComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseComments_AspNetUsers_LastModifiedById",
                table: "ReportingCaseComments");

            migrationBuilder.DropIndex(
                name: "IX_ReportingCaseComments_CreatedById",
                table: "ReportingCaseComments");

            migrationBuilder.DropIndex(
                name: "IX_ReportingCaseComments_LastModifiedById",
                table: "ReportingCaseComments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ReportingCaseComments");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "ReportingCaseComments");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ReportingCaseComments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ReportingCaseComments",
                type: "text",
                nullable: true);
        }
    }
}
