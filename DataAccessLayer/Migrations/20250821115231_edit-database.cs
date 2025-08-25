using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class editdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Ledgers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ledgers_OwnerId",
                table: "Ledgers",
                newName: "IX_Ledgers_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_AspNetUsers_UserId",
                table: "Ledgers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_AspNetUsers_UserId",
                table: "Ledgers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Ledgers",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Ledgers_UserId",
                table: "Ledgers",
                newName: "IX_Ledgers_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
