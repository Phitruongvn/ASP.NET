using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NguyenPhiTruong_2122110563.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceSaleAndContentToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceSale",
                table: "Products",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PriceSale",
                table: "Products");
        }
    }
}
