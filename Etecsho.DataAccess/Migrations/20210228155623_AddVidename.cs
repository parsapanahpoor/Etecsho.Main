using Microsoft.EntityFrameworkCore.Migrations;

namespace Etecsho.DataAccess.Migrations
{
    public partial class AddVidename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoName",
                table: "Blog",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoName",
                table: "Blog");
        }
    }
}
