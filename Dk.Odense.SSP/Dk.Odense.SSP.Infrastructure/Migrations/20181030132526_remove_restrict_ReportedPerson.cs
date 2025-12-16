using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class remove_restrict_ReportedPerson : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries");

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries",
                column: "ReportedPerson_Id",
                principalTable: "ReportedPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries");

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries",
                column: "ReportedPerson_Id",
                principalTable: "ReportedPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
