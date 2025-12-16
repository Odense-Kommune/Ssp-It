using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class delete_behavior_test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worries_Persons_Person_Id",
                table: "Worries");

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_Persons_Person_Id",
                table: "Worries",
                column: "Person_Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worries_Persons_Person_Id",
                table: "Worries");

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_Persons_Person_Id",
                table: "Worries",
                column: "Person_Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
