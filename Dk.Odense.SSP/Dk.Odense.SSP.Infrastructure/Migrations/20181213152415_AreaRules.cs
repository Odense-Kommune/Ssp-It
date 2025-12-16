using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class AreaRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AreaRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    System = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    SearchValue = table.Column<string>(nullable: true),
                    SspArea_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaRules_SspAreas_SspArea_Id",
                        column: x => x.SspArea_Id,
                        principalTable: "SspAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AreaRules_SspArea_Id",
                table: "AreaRules",
                column: "SspArea_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AreaRules");
        }
    }
}
