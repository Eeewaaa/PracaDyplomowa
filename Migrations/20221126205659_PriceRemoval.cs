using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate3.Migrations
{
    /// <inheritdoc />
    public partial class PriceRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ShoppingCartItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "ShoppingCartItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
