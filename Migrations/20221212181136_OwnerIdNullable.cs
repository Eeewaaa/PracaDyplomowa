using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate3.Migrations
{
    /// <inheritdoc />
    public partial class OwnerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estates_Owners_OwnerId",
                table: "Estates");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Estates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Estates_Owners_OwnerId",
                table: "Estates",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estates_Owners_OwnerId",
                table: "Estates");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Estates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estates_Owners_OwnerId",
                table: "Estates",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
