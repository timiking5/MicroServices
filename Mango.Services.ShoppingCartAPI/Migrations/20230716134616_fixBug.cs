using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Headers_CardHeaderId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_CardHeaderId",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "CardHeaderId",
                table: "Details");

            migrationBuilder.CreateIndex(
                name: "IX_Details_CartHeaderId",
                table: "Details",
                column: "CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Headers_CartHeaderId",
                table: "Details",
                column: "CartHeaderId",
                principalTable: "Headers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Headers_CartHeaderId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_CartHeaderId",
                table: "Details");

            migrationBuilder.AddColumn<int>(
                name: "CardHeaderId",
                table: "Details",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Details_CardHeaderId",
                table: "Details",
                column: "CardHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Headers_CardHeaderId",
                table: "Details",
                column: "CardHeaderId",
                principalTable: "Headers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
