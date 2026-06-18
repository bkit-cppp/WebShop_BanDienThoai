using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebEcomerceStoreAPI.Migrations
{
    /// <inheritdoc />
    public partial class editProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Product_ProductId1",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ProductId1",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "Product");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductImages_Product_ProductId",
                table: "ProductImages",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImages_Product_ProductId",
                table: "ProductImages");

            migrationBuilder.DropIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductId1",
                table: "Product",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Product_ProductId1",
                table: "Product",
                column: "ProductId1",
                principalTable: "Product",
                principalColumn: "ProductId");
        }
    }
}
