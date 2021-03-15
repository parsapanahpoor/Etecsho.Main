using Microsoft.EntityFrameworkCore.Migrations;

namespace Etecsho.DataAccess.Migrations
{
    public partial class initialGalleryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductGallery",
                columns: table => new
                {
                    ProductGalleryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 800, nullable: false),
                    ImageName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGallery", x => x.ProductGalleryId);
                    table.ForeignKey(
                        name: "FK_ProductGallery_product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGallery_ProductID",
                table: "ProductGallery",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGallery");
        }
    }
}
