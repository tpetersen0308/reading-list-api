using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace reading_list_api.Migrations
{
    public partial class CapitalizeBookColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "avatar",
                table: "Users",
                newName: "Avatar");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Books",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "authors",
                table: "Books",
                newName: "Authors");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Books",
                newName: "BookId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Books",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Avatar",
                table: "Users",
                newName: "avatar");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Books",
                newName: "image");

            migrationBuilder.RenameColumn(
                name: "Authors",
                table: "Books",
                newName: "authors");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "id");
        }
    }
}
