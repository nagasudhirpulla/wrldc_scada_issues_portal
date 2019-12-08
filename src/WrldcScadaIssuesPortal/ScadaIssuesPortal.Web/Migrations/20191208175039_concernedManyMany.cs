using Microsoft.EntityFrameworkCore.Migrations;

namespace ScadaIssuesPortal.Web.Migrations
{
    public partial class concernedManyMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseConcerned_ReportingCases_ReportingCaseId",
                table: "ReportingCaseConcerned");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseConcerned_AspNetUsers_UserId",
                table: "ReportingCaseConcerned");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportingCaseConcerned",
                table: "ReportingCaseConcerned");

            migrationBuilder.RenameTable(
                name: "ReportingCaseConcerned",
                newName: "ReportingCaseConcerneds");

            migrationBuilder.RenameIndex(
                name: "IX_ReportingCaseConcerned_UserId",
                table: "ReportingCaseConcerneds",
                newName: "IX_ReportingCaseConcerneds_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportingCaseConcerneds",
                table: "ReportingCaseConcerneds",
                columns: new[] { "ReportingCaseId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseConcerneds_ReportingCases_ReportingCaseId",
                table: "ReportingCaseConcerneds",
                column: "ReportingCaseId",
                principalTable: "ReportingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseConcerneds_AspNetUsers_UserId",
                table: "ReportingCaseConcerneds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseConcerneds_ReportingCases_ReportingCaseId",
                table: "ReportingCaseConcerneds");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportingCaseConcerneds_AspNetUsers_UserId",
                table: "ReportingCaseConcerneds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportingCaseConcerneds",
                table: "ReportingCaseConcerneds");

            migrationBuilder.RenameTable(
                name: "ReportingCaseConcerneds",
                newName: "ReportingCaseConcerned");

            migrationBuilder.RenameIndex(
                name: "IX_ReportingCaseConcerneds_UserId",
                table: "ReportingCaseConcerned",
                newName: "IX_ReportingCaseConcerned_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportingCaseConcerned",
                table: "ReportingCaseConcerned",
                columns: new[] { "ReportingCaseId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseConcerned_ReportingCases_ReportingCaseId",
                table: "ReportingCaseConcerned",
                column: "ReportingCaseId",
                principalTable: "ReportingCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportingCaseConcerned_AspNetUsers_UserId",
                table: "ReportingCaseConcerned",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
