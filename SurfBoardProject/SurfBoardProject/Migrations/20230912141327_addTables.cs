using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfBoardProject.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "TotalPrice",
                table: "Rental");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "RentalId",
                table: "BoardModel");

            migrationBuilder.CreateTable(
                name: "BoardModelRental",
                columns: table => new
                {
                    BoardsId = table.Column<int>(type: "int", nullable: false),
                    RentalsRentalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardModelRental", x => new { x.BoardsId, x.RentalsRentalId });
                    table.ForeignKey(
                        name: "FK_BoardModelRental_BoardModel_BoardsId",
                        column: x => x.BoardsId,
                        principalTable: "BoardModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardModelRental_Rental_RentalsRentalId",
                        column: x => x.RentalsRentalId,
                        principalTable: "Rental",
                        principalColumn: "RentalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRental",
                columns: table => new
                {
                    CustomersCustomerId = table.Column<int>(type: "int", nullable: false),
                    RentalsRentalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRental", x => new { x.CustomersCustomerId, x.RentalsRentalId });
                    table.ForeignKey(
                        name: "FK_CustomerRental_Customer_CustomersCustomerId",
                        column: x => x.CustomersCustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRental_Rental_RentalsRentalId",
                        column: x => x.RentalsRentalId,
                        principalTable: "Rental",
                        principalColumn: "RentalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardModelRental_RentalsRentalId",
                table: "BoardModelRental",
                column: "RentalsRentalId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRental_RentalsRentalId",
                table: "CustomerRental",
                column: "RentalsRentalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardModelRental");

            migrationBuilder.DropTable(
                name: "CustomerRental");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Rental",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RentalId",
                table: "Customer",
                type: "int",
                nullable: true);

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
                principalColumn: "RentalId");
        }
    }
}
