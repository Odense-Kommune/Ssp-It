using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class cascade_delete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_SchoolData_Persons_Person_Id",
                table: "SchoolData");

            migrationBuilder.DropForeignKey(
                name: "FK_Worries_Concerns_Concern_Id",
                table: "Worries");

            migrationBuilder.DropForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries");

            migrationBuilder.DropIndex(
                name: "IX_Worries_Concern_Id",
                table: "Worries");

            migrationBuilder.DropIndex(
                name: "IX_SchoolData_Person_Id",
                table: "SchoolData");

            migrationBuilder.DropIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Person_Id",
                table: "SchoolData");

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Concern_Id",
                table: "Worries",
                column: "Concern_Id",
                unique: true,
                filter: "[Concern_Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id",
                unique: true,
                filter: "[SchoolData_Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id",
                principalTable: "SchoolData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_Concerns_Concern_Id",
                table: "Worries",
                column: "Concern_Id",
                principalTable: "Concerns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries",
                column: "ReportedPerson_Id",
                principalTable: "ReportedPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Worries_Concerns_Concern_Id",
                table: "Worries");

            migrationBuilder.DropForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries");

            migrationBuilder.DropIndex(
                name: "IX_Worries_Concern_Id",
                table: "Worries");

            migrationBuilder.DropIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons");

            migrationBuilder.AddColumn<Guid>(
                name: "Person_Id",
                table: "SchoolData",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Worries_Concern_Id",
                table: "Worries",
                column: "Concern_Id");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolData_Person_Id",
                table: "SchoolData",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_SchoolData_SchoolData_Id",
                table: "Persons",
                column: "SchoolData_Id",
                principalTable: "SchoolData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchoolData_Persons_Person_Id",
                table: "SchoolData",
                column: "Person_Id",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_Concerns_Concern_Id",
                table: "Worries",
                column: "Concern_Id",
                principalTable: "Concerns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worries_ReportedPersons_ReportedPerson_Id",
                table: "Worries",
                column: "ReportedPerson_Id",
                principalTable: "ReportedPersons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
