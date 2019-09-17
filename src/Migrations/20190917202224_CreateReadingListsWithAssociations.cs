using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace reading_list_api.Migrations
{
    public partial class CreateReadingListsWithAssociations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReadingListId",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReadingLists",
                columns: table => new
                {
                    ReadingListId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadingLists", x => x.ReadingListId);
                    table.ForeignKey(
                        name: "FK_ReadingLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReadingListId",
                table: "Books",
                column: "ReadingListId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingLists_UserId",
                table: "ReadingLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books",
                column: "ReadingListId",
                principalTable: "ReadingLists",
                principalColumn: "ReadingListId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "ReadingLists");

            migrationBuilder.DropIndex(
                name: "IX_Books_ReadingListId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ReadingListId",
                table: "Books");
        }
    }
}
