using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class AddedClassification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Classification_Id",
                table: "PersonGroupings",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classification", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonGroupings_Classification_Id",
                table: "PersonGroupings",
                column: "Classification_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PersonGroupings_Classification_Classification_Id",
                table: "PersonGroupings",
                column: "Classification_Id",
                principalTable: "Classification",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonGroupings_Classification_Classification_Id",
                table: "PersonGroupings");

            migrationBuilder.DropTable(
                name: "Classification");

            migrationBuilder.DropIndex(
                name: "IX_PersonGroupings_Classification_Id",
                table: "PersonGroupings");

            migrationBuilder.DropColumn(
                name: "Classification_Id",
                table: "PersonGroupings");
        }
    }
}
