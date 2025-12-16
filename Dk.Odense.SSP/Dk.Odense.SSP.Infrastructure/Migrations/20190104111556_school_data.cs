using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class school_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastVerified",
                table: "Persons",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "SchoolData_Id",
                table: "Persons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchoolData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Person_Id = table.Column<Guid>(nullable: false),
                    DateFirst = table.Column<DateTime>(nullable: false),
                    DateLast = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolData_Persons_Person_Id",
                        column: x => x.Person_Id,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolData_Person_Id",
                table: "SchoolData",
                column: "Person_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id",
                principalTable: "SchoolData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons");

            migrationBuilder.DropTable(
                name: "SchoolData");

            migrationBuilder.DropIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "LastVerified",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "SchoolData_Id",
                table: "Persons");
        }
    }
}
