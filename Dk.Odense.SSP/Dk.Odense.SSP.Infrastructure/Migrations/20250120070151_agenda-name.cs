using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class agendaname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgendaName",
                table: "Agendas",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgendaName",
                table: "Agendas");
        }
    }
}
