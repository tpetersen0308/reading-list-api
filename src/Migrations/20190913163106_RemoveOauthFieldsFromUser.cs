using Microsoft.EntityFrameworkCore.Migrations;

namespace reading_list_api.Migrations
{
    public partial class RemoveOauthFieldsFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oauthIssuer",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "oauthSubject",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "oauthIssuer",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "oauthSubject",
                table: "Users",
                nullable: true);
        }
    }
}
