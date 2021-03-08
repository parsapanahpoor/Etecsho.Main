using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Etecsho.DataAccess.Migrations
{
    public partial class VideoTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_blogSelectedCategories_blogCategories_BlogCategoryId",
                table: "blogSelectedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_blogSelectedCategories_Blog_BlogId",
                table: "blogSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blogSelectedCategories",
                table: "blogSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_blogCategories",
                table: "blogCategories");

            migrationBuilder.RenameTable(
                name: "blogSelectedCategories",
                newName: "BlogSelectedCategories");

            migrationBuilder.RenameTable(
                name: "blogCategories",
                newName: "BlogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_blogSelectedCategories_BlogId",
                table: "BlogSelectedCategories",
                newName: "IX_BlogSelectedCategories_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_blogSelectedCategories_BlogCategoryId",
                table: "BlogSelectedCategories",
                newName: "IX_BlogSelectedCategories_BlogCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogSelectedCategories",
                table: "BlogSelectedCategories",
                column: "BlogSelectedCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories",
                column: "BlogCategoryId");

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    VideoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    VideoTitle = table.Column<string>(maxLength: 450, nullable: false),
                    ShortDescription = table.Column<string>(nullable: false),
                    LongDescription = table.Column<string>(nullable: false),
                    VideoImageName = table.Column<string>(maxLength: 50, nullable: true),
                    DemoFileName = table.Column<string>(nullable: true),
                    Tags = table.Column<string>(maxLength: 600, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsAparat = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.VideoId);
                    table.ForeignKey(
                        name: "FK_Video_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoSelectedCategory",
                columns: table => new
                {
                    VideoSelectedCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogCategoryId = table.Column<int>(nullable: false),
                    VideoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoSelectedCategory", x => x.VideoSelectedCategoryId);
                    table.ForeignKey(
                        name: "FK_VideoSelectedCategory_BlogCategories_BlogCategoryId",
                        column: x => x.BlogCategoryId,
                        principalTable: "BlogCategories",
                        principalColumn: "BlogCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VideoSelectedCategory_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Video_UserId",
                table: "Video",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoSelectedCategory_BlogCategoryId",
                table: "VideoSelectedCategory",
                column: "BlogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoSelectedCategory_VideoId",
                table: "VideoSelectedCategory",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSelectedCategories_BlogCategories_BlogCategoryId",
                table: "BlogSelectedCategories",
                column: "BlogCategoryId",
                principalTable: "BlogCategories",
                principalColumn: "BlogCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogSelectedCategories_Blog_BlogId",
                table: "BlogSelectedCategories",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogSelectedCategories_BlogCategories_BlogCategoryId",
                table: "BlogSelectedCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogSelectedCategories_Blog_BlogId",
                table: "BlogSelectedCategories");

            migrationBuilder.DropTable(
                name: "VideoSelectedCategory");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogSelectedCategories",
                table: "BlogSelectedCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories");

            migrationBuilder.RenameTable(
                name: "BlogSelectedCategories",
                newName: "blogSelectedCategories");

            migrationBuilder.RenameTable(
                name: "BlogCategories",
                newName: "blogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_BlogSelectedCategories_BlogId",
                table: "blogSelectedCategories",
                newName: "IX_blogSelectedCategories_BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogSelectedCategories_BlogCategoryId",
                table: "blogSelectedCategories",
                newName: "IX_blogSelectedCategories_BlogCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blogSelectedCategories",
                table: "blogSelectedCategories",
                column: "BlogSelectedCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_blogCategories",
                table: "blogCategories",
                column: "BlogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_blogSelectedCategories_blogCategories_BlogCategoryId",
                table: "blogSelectedCategories",
                column: "BlogCategoryId",
                principalTable: "blogCategories",
                principalColumn: "BlogCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_blogSelectedCategories_Blog_BlogId",
                table: "blogSelectedCategories",
                column: "BlogId",
                principalTable: "Blog",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
