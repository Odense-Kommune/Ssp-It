using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class RenameClassification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonGroupings_Classification_Classification_Id",
                table: "PersonGroupings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classification",
                table: "Classification");

            migrationBuilder.RenameTable(
                name: "Classification",
                newName: "Classifications");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonGroupings_Classifications_Classification_Id",
                table: "PersonGroupings",
                column: "Classification_Id",
                principalTable: "Classifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonGroupings_Classifications_Classification_Id",
                table: "PersonGroupings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classifications",
                table: "Classifications");

            migrationBuilder.RenameTable(
                name: "Classifications",
                newName: "Classification");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classification",
                table: "Classification",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonGroupings_Classification_Classification_Id",
                table: "PersonGroupings",
                column: "Classification_Id",
                principalTable: "Classification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
