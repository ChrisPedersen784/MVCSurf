using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SurfBoardProject.Migrations
{
    /// <inheritdoc />
    public partial class addIdentityUserToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Customer",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customer_IdentityUserId",
                table: "Customer",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_AspNetUsers_IdentityUserId",
                table: "Customer",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_AspNetUsers_IdentityUserId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_Customer_IdentityUserId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Customer");
        }
    }
}
