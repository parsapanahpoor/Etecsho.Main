using Microsoft.EntityFrameworkCore.Migrations;

namespace Etecsho.DataAccess.Migrations
{
    public partial class Initialproductcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryTitle = table.Column<string>(maxLength: 200, nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProductSelectedCategory",
                columns: table => new
                {
                    ProductSelectedCategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryId = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    ProductCategoriesProductCategoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSelectedCategory", x => x.ProductSelectedCategoryID);
                    table.ForeignKey(
                        name: "FK_ProductSelectedCategory_ProductCategories_ProductCategoriesProductCategoryId",
                        column: x => x.ProductCategoriesProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedCategory_ProductCategoriesProductCategoryId",
                table: "ProductSelectedCategory",
                column: "ProductCategoriesProductCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSelectedCategory");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
