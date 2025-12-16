using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class SocialWorkernotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SocialWorker",
                table: "Persons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Notes",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SocialWorker",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Notes");
        }
    }
}
