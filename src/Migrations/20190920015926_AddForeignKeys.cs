using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace reading_list_api.Migrations
{
    public partial class AddForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingLists_Users_UserId",
                table: "ReadingLists");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ReadingLists",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ReadingListId",
                table: "Books",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books",
                column: "ReadingListId",
                principalTable: "ReadingLists",
                principalColumn: "ReadingListId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingLists_Users_UserId",
                table: "ReadingLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingLists_Users_UserId",
                table: "ReadingLists");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ReadingLists",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ReadingListId",
                table: "Books",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Books_ReadingLists_ReadingListId",
                table: "Books",
                column: "ReadingListId",
                principalTable: "ReadingLists",
                principalColumn: "ReadingListId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingLists_Users_UserId",
                table: "ReadingLists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
