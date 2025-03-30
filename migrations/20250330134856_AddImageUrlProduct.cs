using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webquanlydouong.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Img",
                table: "Products",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "Img");
        }
    }
}
