using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEcomerceStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class EditReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_UsersUserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UsersUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_User_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.AddColumn<Guid>(
                name: "UsersUserId",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UsersUserId",
                table: "Reviews",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_User_UsersUserId",
                table: "Reviews",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
