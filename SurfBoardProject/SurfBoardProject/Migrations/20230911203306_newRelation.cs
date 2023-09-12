using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfBoardProject.Migrations
{
    /// <inheritdoc />
    public partial class newRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "BoardModel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RentalId",
                table: "Customer",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardModel_RentalId",
                table: "BoardModel",
                column: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardModel_Rental_RentalId",
                table: "BoardModel",
                column: "RentalId",
                principalTable: "Rental",
                principalColumn: "RentalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Rental_RentalId",
                table: "Customer",
                column: "RentalId",
                principalTable: "Rental",
                principalColumn: "RentalId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardModel_Rental_RentalId",
                table: "BoardModel");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Rental_RentalId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_RentalId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_BoardModel_RentalId",
                table: "BoardModel");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "BoardModel");
        }
    }
}
