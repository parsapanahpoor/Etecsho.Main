using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Etecsho.DataAccess.Migrations
{
    public partial class inirialProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ProductTitle = table.Column<string>(maxLength: 450, nullable: false),
                    ShortDescription = table.Column<string>(nullable: false),
                    LongDescription = table.Column<string>(nullable: false),
                    ProductImageName = table.Column<string>(maxLength: 50, nullable: true),
                    OfferPercent = table.Column<int>(nullable: true),
                    IsInOffer = table.Column<bool>(nullable: true),
                    ProductCount = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    Tags = table.Column<string>(maxLength: 600, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_product_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedCategory_ProductID",
                table: "ProductSelectedCategory",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_product_UserId",
                table: "product",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSelectedCategory_product_ProductID",
                table: "ProductSelectedCategory",
                column: "ProductID",
                principalTable: "product",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSelectedCategory_product_ProductID",
                table: "ProductSelectedCategory");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropIndex(
                name: "IX_ProductSelectedCategory_ProductID",
                table: "ProductSelectedCategory");
        }
    }
}
