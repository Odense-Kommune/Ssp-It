using Microsoft.EntityFrameworkCore.Migrations;

namespace Dk.Odense.SSP.Infrastructure.Migrations
{
    public partial class AddedReplyRecipitent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Robustnesses",
                newName: "CreatedDate");

            migrationBuilder.AddColumn<string>(
                name: "ReplyRecipientMail",
                table: "Robustnesses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReplyRecipientName",
                table: "Robustnesses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyRecipientMail",
                table: "Robustnesses");

            migrationBuilder.DropColumn(
                name: "ReplyRecipientName",
                table: "Robustnesses");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Robustnesses",
                newName: "Date");
        }
    }
}
